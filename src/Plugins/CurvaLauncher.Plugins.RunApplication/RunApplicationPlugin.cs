using System.Windows.Media;
using System.IO;
using CurvaLauncher.Plugins.RunApplication.Properties;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Xml;
using CurvaLauncher.Apis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Diagnostics;
using CurvaLauncher.Plugins.RunApplication.RegistryHelper;
using CurvaLauncher.Plugins.RunApplication.Pinyin;

namespace CurvaLauncher.Plugins.RunApplication;

public class RunApplicationPlugin : AsyncI18nPlugin
{
    char[] displayNameBuffer = new char[260];

    readonly string resourcePrefix = "ms-resource:";
    readonly string uwpPackageRepositoryRegistry =
        @"Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages";

    [PluginI18nOption("StrResultCount")]
    public int ResultCount { get; set; } = 5;
    
    [PluginI18nOption("StrIgnoreCases")]
    public bool IgnoreCases { get; set; } = true;

    [PluginI18nOption("StrEnableFileNameSearch")]
    public bool EnableFileNameSearch { get; set; } = true;

    [PluginI18nOption("StrEnablePinyinSearch")]
    public bool EnablePinyinSearch { get; set; } = false;


    [PluginI18nOption("StrIndexLocations")]
    public IndexLocations IndexLocations { get; set; } =
        IndexLocations.UWP |
        IndexLocations.CommonPrograms |
        IndexLocations.Programs |
        IndexLocations.Desktop;

    [PluginI18nOption("StrWin32AppDistinctMode")]
    public Win32AppDistinctMode Win32AppDistinctMode { get; set; } = Win32AppDistinctMode.FilePath;

    [PluginI18nOption("StrCustomIndexFolders", AllowTextMultiline = true)]
    public string CustomFolders { get; set; } = string.Empty;

    [PluginI18nOption("StrRegexsForExcludingApps", AllowTextMultiline = true)]
    public string RegexsForExcludingApps { get; set; } = "^[Uu]ninstall";

    public override ImageSource Icon { get; }

    public override object NameKey => "StrPluginName";
    public override object DescriptionKey => "StrPluginDescription";


    private List<AppInfo>? _apps;
    private List<FileSystemWatcher>? _win32FileSystemWatchers;
    private RegistryMonitor? _uwpRegistryMonitor;

    private HashSet<string>? _uwpRepositorySubKeyNames = null;

    private PinyinDictionary? _pinyinDictionary = null;


    public RunApplicationPlugin(CurvaLauncherContext context) : base(context)
    {
        Icon = HostContext.ImageApi.CreateFromSvg(Resources.IconSvg)!;
    }

    public override Task InitializeAsync()
    {
        return Task.Run(() =>
        {
            _apps = new();

            if (EnablePinyinSearch)
            {
                _pinyinDictionary = new();
            }

            InitializeWin32();
            InitializeUwp();

            InitializeWin32Watch();
            InitializeUwpWatch();

            _pinyinDictionary = null;
        });
    }

    private Win32AppInfo? GetWin32App(string shortcut)
    {
        if (HostContext.FileApi.GetShortcutTarget(shortcut) is not ShortcutTarget target)
            return null;

        var ext = Path.GetExtension(target.FileName);
        if (!string.Equals(ext, ".exe", StringComparison.OrdinalIgnoreCase))
            return null;
        if (!File.Exists(Environment.ExpandEnvironmentVariables(target.FileName)))
            return null;

        var name = Path.GetFileNameWithoutExtension(shortcut);

        return new Win32AppInfo()
        {
            Name = name,
            FilePath = target.FileName,
            Arguments = target.Arguments,
            WorkingDirectory = target.WorkingDirectory,
            IconPath = target.IconPath,
            IconIndex = target.IconIndex,
            IsUAC = target.RequiresAdmin,

            OriginShortcutPath = shortcut
        };
    }

    private void InitializeWin32()
    {
        if (_apps == null)
            return;

        var allShotcutsInStartMenu = Enumerable.Empty<string>();

        if (IndexLocations.HasFlag(IndexLocations.CommonPrograms))
            allShotcutsInStartMenu = allShotcutsInStartMenu.Concat(
                Directory.EnumerateFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), "*.lnk", SearchOption.AllDirectories));

        if (IndexLocations.HasFlag(IndexLocations.Programs))
            allShotcutsInStartMenu = allShotcutsInStartMenu.Concat(
                Directory.EnumerateFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.Programs), "*.lnk", SearchOption.AllDirectories));

        if (IndexLocations.HasFlag(IndexLocations.Desktop))
            allShotcutsInStartMenu = allShotcutsInStartMenu.Concat(
                Directory.EnumerateFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "*.lnk", SearchOption.TopDirectoryOnly));

        if (!string.IsNullOrWhiteSpace(CustomFolders))
        {
            foreach (string customFolder in CustomFolders.Split('\r', '\n', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!Directory.Exists(customFolder))
                    continue;

                allShotcutsInStartMenu = allShotcutsInStartMenu.Concat(
                    Directory.EnumerateFiles(customFolder, "*.lnk", SearchOption.TopDirectoryOnly));
            }
        }

        List<Regex>? regexsForExcludingApps = null;
        if (!string.IsNullOrWhiteSpace(RegexsForExcludingApps))
        {
            regexsForExcludingApps = new();
            foreach (string regexStr in RegexsForExcludingApps.Split('\r', '\n', StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    regexsForExcludingApps.Add(new Regex(regexStr));
                }
                catch { }
            }
        }


        List<string> alterQueryRoots = new();

        foreach (var shortcut in allShotcutsInStartMenu)
        {
            if (GetWin32App(shortcut) is Win32AppInfo newApp &&
                !_apps.OfType<Win32AppInfo>().Any(app => app.IsSame(newApp, Win32AppDistinctMode)))
            {
                if (regexsForExcludingApps != null && regexsForExcludingApps.Any(r => r.IsMatch(newApp.Name)))
                    continue;

                _apps.Add(newApp);

                alterQueryRoots.Clear();

                if (EnableFileNameSearch)
                {
                    alterQueryRoots.Add(System.IO.Path.GetFileNameWithoutExtension(newApp.FilePath));
                }

                if (EnablePinyinSearch)
                {
                    alterQueryRoots.AddRange(_pinyinDictionary!.GetAllPinyins(newApp.Name).Select(pinyin => string.Concat(pinyin)));
                }

                if (alterQueryRoots.Count > 0)
                {
                    newApp.AlterQueryRoots = alterQueryRoots.ToArray();
                }

                if (IgnoreCases)
                {
                    newApp.QueryRoot = newApp.Name.ToLower();
                }
            }
        }
    }

    private void InitializeWin32Watch()
    {
        _win32FileSystemWatchers = new();

        List<string> folders = new(3);

        if (IndexLocations.HasFlag(IndexLocations.CommonPrograms))
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
            var watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
            };

            watcher.Created += Win32AppWatcher_Created;
            watcher.Deleted += Win32AppWatcher_Deleted;
            _win32FileSystemWatchers.Add(watcher);
        }

        if (IndexLocations.HasFlag(IndexLocations.Programs))
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            var watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true,
            };

            watcher.Created += Win32AppWatcher_Created;
            watcher.Deleted += Win32AppWatcher_Deleted;
            _win32FileSystemWatchers.Add(watcher);
        }

        if (IndexLocations.HasFlag(IndexLocations.Desktop))
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var watcher = new FileSystemWatcher(path)
            {
                EnableRaisingEvents = true,
            };

            watcher.Created += Win32AppWatcher_Created;
            watcher.Deleted += Win32AppWatcher_Deleted;
            watcher.Renamed += Win32AppWatcher_Renamed;
            watcher.Changed += Win32AppWatcher_Changed;
            _win32FileSystemWatchers.Add(watcher);
        }
    }

    private void Win32AppWatcher_Created(object sender, FileSystemEventArgs e)
    {
        if (_apps == null)
            return;

        lock (_apps)
        {
            if (e.FullPath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase) &&
                GetWin32App(e.FullPath) is Win32AppInfo win32AppInfo)
            {
                _apps.Add(win32AppInfo);

                Debug.WriteLine("Win32 app added");
            }
        }
    }

    private void Win32AppWatcher_Deleted(object sender, FileSystemEventArgs e)
    {
        if (_apps == null)
            return;

        lock (_apps)
        {
            var appIndex = _apps
            .FindIndex(app => app is Win32AppInfo win32App && win32App.OriginShortcutPath == e.FullPath);

            if (appIndex is not -1)
            {
                _apps.RemoveAt(appIndex);

                Debug.WriteLine("Win32 app deleted");
            }
        }
    }

    private void Win32AppWatcher_Renamed(object sender, RenamedEventArgs e)
    {
        if (_apps == null)
            return;

        lock (_apps)
        {
            var app = _apps
                .OfType<Win32AppInfo>()
                .FirstOrDefault(app => app.OriginShortcutPath == e.OldFullPath);

            if (app is not null)
            {
                app.Name = Path.GetFileNameWithoutExtension(e.FullPath);
                app.OriginShortcutPath = e.FullPath;

                Debug.WriteLine("Win32 app renamed");
            }
        }
    }

    private void Win32AppWatcher_Changed(object sender, FileSystemEventArgs e)
    {
        if (_apps == null)
            return;

        lock (_apps)
        {
            var app = _apps
                .OfType<Win32AppInfo>()
                .FirstOrDefault(app => app.OriginShortcutPath == e.FullPath);

            if (app is not null)
            {
                if (GetWin32App(e.FullPath) is Win32AppInfo newInfo)
                    app.Populate(newInfo);
                else
                    _apps.Remove(app);
            }
            else
            {
                if (e.FullPath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase) &&
                    GetWin32App(e.FullPath) is Win32AppInfo newInfo)
                    _apps.Add(newInfo);
            }
        }
    }

    private IEnumerable<UwpAppInfo> GetUwpApps(RegistryKey subKey)
    {
        if (subKey.GetValue("PackageID") is not string packageId)
            yield break;

        string packageFamilyId = Regex.Replace(packageId, "_.*__", "_");

        if (subKey.GetValue("PackageRootFolder") is not string packageRootFolder)
            yield break;

        string packageManifestPath = Path.Combine(packageRootFolder, "AppxManifest.xml");

        if (!System.IO.File.Exists(packageManifestPath))
            yield break;

        var appxManifestContent = File.ReadAllText(packageManifestPath);

        XmlDocument xml = new();
        xml.LoadXml(appxManifestContent);

        XmlNamespaceManager nsManager = new XmlNamespaceManager(xml.NameTable);//这一步实例化一个xml命名空间管理器
        nsManager.AddNamespace("ns", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
        nsManager.AddNamespace("uap", "http://schemas.microsoft.com/appx/manifest/uap/windows10");

        var idNode = xml.SelectSingleNode("/ns:Package/ns:Identity", nsManager);
        var appNodes = xml.SelectNodes("/ns:Package/ns:Applications/ns:Application", nsManager);

        if (appNodes == null)
            yield break;

        foreach (XmlNode appNode in appNodes)
        {
            var visualElementsNode = appNode.SelectSingleNode("uap:VisualElements", nsManager);

            if (visualElementsNode == null)
                continue;

            if (visualElementsNode?.Attributes?["AppListEntry"]?.Value is string appListEntry &&
                appListEntry.Equals("none", StringComparison.OrdinalIgnoreCase))
                continue;

            if (visualElementsNode?.Attributes?["DisplayName"]?.Value is not string displayName)
                continue;

            UwpAppInfo info = new()
            {
                OriginRegistryKeyName = Path.GetFileName(subKey.Name)
            };

            info.PackageId = packageId;
            info.FamilyID = packageFamilyId;
            info.PackageRootFolder = packageRootFolder;

            var logoNode = xml.SelectSingleNode("/ns:Package/ns:Properties/ns:Logo", nsManager);

            if (appNode == null ||
                appNode.Attributes?["Id"]?.Value is not string appId)
                continue;

            info.ApplicationId = appId;

            if (displayName.StartsWith(resourcePrefix))
            {
                string resourcePath = displayName.Substring(resourcePrefix.Length);

                // 非绝对路径, 且能够找到 Identity 节点
                if (!resourcePath.StartsWith("//") &&
                    idNode?.Attributes?["Name"]?.Value is string id)
                {
                    // 不是引用其他资源, 则添加 'Resource' 前缀
                    if (!resourcePath.Contains("Resources"))
                        resourcePath = $"Resources/{resourcePath}";

                    // 转为绝对资源
                    resourcePath = $"//{id}/{resourcePath}";
                }

                string resourceStr = $"@{{{packageId}?ms-resource:{resourcePath}}}";
                uint errCode =SHLoadIndirectString(resourceStr, ref displayNameBuffer[0], displayNameBuffer.Length, 0);

                int endIndex = Array.IndexOf(displayNameBuffer, '\0');
                displayName = new string(displayNameBuffer, 0, endIndex);

                if (errCode != 0)
                    continue;
            }

            if (string.IsNullOrWhiteSpace(displayName))
                continue;

            info.Name = displayName;

            string? logoPath = logoNode?.InnerText;

            if (logoPath is not null)
            {
                var logoFullPath = Path.Combine(info.PackageRootFolder, logoPath);

                var logoFileName = Path.GetFileNameWithoutExtension(logoPath);
                var logoExtension = Path.GetExtension(logoPath);
                var logoFilesDir = Path.GetDirectoryName(logoFullPath) ?? ".";
                var logoFilesPattern = $"{logoFileName}*{logoExtension}";

                if (Directory.Exists(logoFilesDir))
                {
                    var regex = new Regex($@"{Regex.Escape(logoFileName)}(\.scale-(?<scale>\d+))?{Regex.Escape(logoExtension)}");

                    List<UwpAppInfo.UwpAppLogo> logos = new();
                    foreach (var searchedLogoFile in Directory.EnumerateFiles(logoFilesDir, logoFilesPattern))
                    {
                        var searchedLogoFileName = Path.GetFileName(searchedLogoFile);
                        var match = regex.Match(searchedLogoFileName);

                        if (!match.Success)
                            continue;

                        int scale = 1;
                        if (int.TryParse(match.Groups["scale"].Value, out var parsedScale))
                            scale = parsedScale;

                        logos.Add(new UwpAppInfo.UwpAppLogo(44 * scale, searchedLogoFile));
                    }

                    info.AppLogos = logos.ToArray();
                }
            }

            yield return info;
        }
    }

    private void InitializeUwp()
    {
        if (_apps == null)
            return;

        if (!IndexLocations.HasFlag(IndexLocations.UWP))
            return;

        using var repositoryKey = Registry.CurrentUser.OpenSubKey(uwpPackageRepositoryRegistry);

        if (repositoryKey is null)
            return;

        _uwpRepositorySubKeyNames = repositoryKey
            .GetSubKeyNames()
            .ToHashSet();

        foreach (var item in _uwpRepositorySubKeyNames)
        {
            using var subKey = repositoryKey.OpenSubKey(item);

            if (subKey is null)
                continue;

            foreach (var app in GetUwpApps(subKey))
            {
                _apps.Add(app);

                if (EnablePinyinSearch)
                {
                    app.AlterQueryRoots = _pinyinDictionary!.GetAllPinyins(app.Name)
                        .Select(pinyin => string.Concat(pinyin))
                        .ToArray();
                }

                if (IgnoreCases)
                {
                    app.QueryRoot = app.Name.ToLower();
                }
            }
          
        }

    }

    private void InitializeUwpWatch()
    {
        if (_apps == null)
            return;

        if (!IndexLocations.HasFlag(IndexLocations.UWP))
            return;

        using var repositoryKey = Registry.CurrentUser.OpenSubKey(uwpPackageRepositoryRegistry);

        if (repositoryKey is null)
            return;

        _uwpRegistryMonitor = new RegistryMonitor(repositoryKey);
        _uwpRegistryMonitor.RegChangeNotifyFilter = RegChangeNotifyFilter.Key;
        _uwpRegistryMonitor.RegChanged += UwpRegistryMonitor_RegChanged;
        _uwpRegistryMonitor.Start();
    }

    private void UwpRegistryMonitor_RegChanged(object? sender, EventArgs e)
    {
        if (_apps == null || _uwpRepositorySubKeyNames == null)
            return;

        using var repositoryKey = Registry.CurrentUser.OpenSubKey(uwpPackageRepositoryRegistry);

        if (repositoryKey is null)
            return;

        var newUwpRepositorySubKeyNames = repositoryKey
            .GetSubKeyNames()
            .ToHashSet();

        if (_uwpRepositorySubKeyNames.Count < newUwpRepositorySubKeyNames.Count)
        {
            foreach (var subKeyOfNew in newUwpRepositorySubKeyNames)
            {
                if (!_uwpRepositorySubKeyNames.Contains(subKeyOfNew))
                {
                    using var subKey = repositoryKey.OpenSubKey(subKeyOfNew);

                    if (subKey is null)
                        continue;

                    lock (_apps)
                    {
                        foreach (var app in GetUwpApps(subKey))
                            _apps.Add(app);
                    }
                }
            }
        }
        else
        {
            foreach (var subKeyOfOld in _uwpRepositorySubKeyNames)
            {
                if (!newUwpRepositorySubKeyNames.Contains(subKeyOfOld))
                {
                    lock (_apps)
                    {
                        int appIndex = _apps
                            .FindIndex(app => app is UwpAppInfo uwpApp && uwpApp.OriginRegistryKeyName == subKeyOfOld);

                        if (appIndex != -1)
                            _apps.RemoveAt(appIndex);
                    }
                }
            }
        }

        _uwpRepositorySubKeyNames = newUwpRepositorySubKeyNames;
    }

    public override Task FinishAsync()
    {
        if (_win32FileSystemWatchers != null)
            foreach (var watcher in _win32FileSystemWatchers)
                watcher.Dispose();

        _apps = null;
        _win32FileSystemWatchers = null;

        return Task.CompletedTask;
    }

    public override async IAsyncEnumerable<IQueryResult> QueryAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            yield break;
        if (_apps == null)
            yield break;

        IEnumerable<(AppInfo App, float Weight)> results;
        var realQuery = IgnoreCases ? query.ToLower() : query;

        lock (_apps)
        {
            results = _apps
                .Select(app => (App: app, Weight: GetAppWeight(app, realQuery)))
                .OrderByDescending(kvw => kvw.Weight)
                .Take(ResultCount);

            foreach (var result in results)
            {
                if (result.App is Win32AppInfo win32AppInfo)
                    yield return new RunWin32ApplicationQueryResult(HostContext, win32AppInfo, result.Weight);
                else if (result.App is UwpAppInfo uwpAppInfo)
                    yield return new RunUwpApplicationQueryResult(HostContext, uwpAppInfo, result.Weight);
            }
        }

        float GetAppWeight(AppInfo appInfo, string realQuery)
        {
            return Math.Max(
                HostContext.StringApi.Match(appInfo.QueryRoot, realQuery),
                appInfo.AlterQueryRoots?.Max(altName => HostContext.StringApi.Match(altName, realQuery)) ?? 0);
        }
    }

    public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
    {
        yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("de"), "I18n/De.xaml");
    }



    [DllImport("shlwapi.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, ThrowOnUnmappableChar = true)]
    static extern unsafe uint SHLoadIndirectString(string pszSource, ref char pszOutBuf, int cchOutBuf, IntPtr ppvReserved);
}
