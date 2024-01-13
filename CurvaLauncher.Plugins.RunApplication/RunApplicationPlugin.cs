using System.Windows.Media;
using System.IO;
using CurvaLauncher.Plugins.RunApplication.Properties;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Xml;
using CurvaLauncher.Apis;
using System.Globalization;
using CurvaLauncher.Plugins.RunApplication.UWP;
using System.Runtime.InteropServices;

namespace CurvaLauncher.Plugins.RunApplication;

public class RunApplicationPlugin : SyncI18nPlugin
{
    [PluginI18nOption("StrResultCount")]
    public int ResultCount { get; set; } = 5;

    [PluginI18nOption("StrIndexLocations")]
    public IndexLocations IndexLocations { get; set; } =
        IndexLocations.UWP |
        IndexLocations.CommonPrograms |
        IndexLocations.Programs |
        IndexLocations.Desktop;

    public override ImageSource Icon { get; }

    public override object NameKey => "StrPluginName";
    public override object DescriptionKey => "StrPluginDescription";


    private Dictionary<string, AppInfo>? _apps;


    public RunApplicationPlugin(CurvaLauncherContext context) : base(context)
    {
        Icon = HostContext.ImageApi.CreateFromSvg(Resources.IconSvg)!;
    }

    public override void Initialize()
    {
        _apps = new();

        InitializeWin32();
        InitializeUwp();
    }

    private void InitializeWin32()
    {
        if (_apps == null)
            return;

        var allShotcutsInStartMenu = new List<string>();

        if (IndexLocations.HasFlag(IndexLocations.CommonPrograms))
            allShotcutsInStartMenu.AddRange(
            Directory.GetFiles(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), "*.lnk", SearchOption.AllDirectories));

        if (IndexLocations.HasFlag(IndexLocations.Programs))
            allShotcutsInStartMenu.AddRange(
                Directory.GetFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.Programs), "*.lnk", SearchOption.AllDirectories));

        if (IndexLocations.HasFlag(IndexLocations.Desktop))
            allShotcutsInStartMenu.AddRange(
                Directory.GetFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "*.lnk", SearchOption.TopDirectoryOnly));

        foreach (var shortcut in allShotcutsInStartMenu)
        {
            if (HostContext.FileApi.GetShortcutTarget(shortcut) is not ShortcutTarget target)
                continue;

            var ext = Path.GetExtension(target.FileName);
            if (!string.Equals(ext, ".exe", StringComparison.OrdinalIgnoreCase))
                continue;
            if (!File.Exists(target.FileName))
                continue;

            var name = Path.GetFileNameWithoutExtension(shortcut);

            _apps[name] = new Win32AppInfo()
            {
                Name = name,
                FilePath = target.FileName,
                Arguments = target.Arguments,
                WorkingDirectory = target.WorkingDirectory,
                IconPath = target.IconPath,
                IconIndex = target.IconIndex,
                IsUAC = target.RequiresAdmin
            };
        }
    }

    private void InitializeUwp()
    {
        if (_apps == null)
            return;

        if (!IndexLocations.HasFlag(IndexLocations.UWP))
            return;

        string resourcePrefix = "ms-resource:";
        char[] displayNameBuffer = new char[260];

        var repositoryKey = Registry.CurrentUser.OpenSubKey(
            @"Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages");

        if (repositoryKey is null)
            return;

        var subKeyNames = repositoryKey.GetSubKeyNames();

        foreach (var item in subKeyNames)
        {
            var subKey = repositoryKey.OpenSubKey(item);

            if (subKey is null)
                continue;

            if (subKey.GetValue("PackageID") is not string packageId)
                continue;

            string packageFamilyId = Regex.Replace(packageId, "_.*__", "_");

            if (subKey.GetValue("PackageRootFolder") is not string packageRootFolder)
                continue;

            string packageManifestPath = Path.Combine(packageRootFolder, "AppxManifest.xml");

            if (!System.IO.File.Exists(packageManifestPath))
                continue;

            var appxManifestContent = File.ReadAllText(packageManifestPath);

            XmlDocument xml = new();
            xml.LoadXml(appxManifestContent);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(xml.NameTable);//这一步实例化一个xml命名空间管理器
            nsManager.AddNamespace("ns", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
            nsManager.AddNamespace("uap", "http://schemas.microsoft.com/appx/manifest/uap/windows10");

            var idNode = xml.SelectSingleNode("/ns:Package/ns:Identity", nsManager);
            var appNodes = xml.SelectNodes("/ns:Package/ns:Applications/ns:Application", nsManager);

            if (appNodes == null)
                continue;

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

                UwpAppInfo info = new();

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

                    uint errCode;
                    string resourceStr = $"@{{{packageId}?ms-resource:{resourcePath}}}";
                    do
                    {
                        errCode = SHLoadIndirectString(resourceStr, ref displayNameBuffer[0], displayNameBuffer.Length, 0);
                    }
                    while (errCode == 1 || errCode == 0x8007007a);

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

                _apps[info.Name] = info;
            }
        }

    }

    public override void Finish()
    {
        _apps = null;
    }

    public override IEnumerable<IQueryResult> Query(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            yield break;
        if (_apps == null)
            yield break;

        var results = _apps
            .Select(kv => (kv.Key, kv.Value, Weight: HostContext.StringApi.Match(kv.Key.ToLower(), query.ToLower())))
            .OrderByDescending(kvw => kvw.Weight)
            .Take(ResultCount);

        foreach (var result in results)
        {
            if (result.Value is Win32AppInfo win32AppInfo)
                yield return new RunWin32ApplicationQueryResult(HostContext, win32AppInfo, result.Weight);
            else if (result.Value is UwpAppInfo uwpAppInfo)
                yield return new RunUwpApplicationQueryResult(HostContext, uwpAppInfo, result.Weight);
        }
    }

    public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
    {
        yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
    }



    [DllImport("shlwapi.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, ThrowOnUnmappableChar = true)]
    static extern unsafe uint SHLoadIndirectString(string pszSource, ref char pszOutBuf, int cchOutBuf, IntPtr ppvReserved);
}
