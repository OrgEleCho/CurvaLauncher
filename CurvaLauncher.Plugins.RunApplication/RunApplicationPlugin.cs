using System.Windows.Media;
using System.IO;
using CurvaLauncher.Plugins.RunApplication.Properties;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Xml;
using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugins.RunApplication;

public class RunApplicationPlugin : CurvaLauncherSyncPlugin
{
    readonly Lazy<ImageSource> laziedIcon;


    [PluginOption]
    public int ResultCount { get; set; } = 5;

    [PluginOption]
    public IndexLocations IndexLocations { get; set; } =
        IndexLocations.AppsFolder |
        IndexLocations.CommonPrograms |
        IndexLocations.Programs |
        IndexLocations.Desktop;

    public override string Name => "Run Application";
    public override string Description => "Run Applications installed on your PC";
    public override ImageSource Icon => laziedIcon.Value;

    static readonly int[] s_uwpIconSizes = [44, 150, 310];


    private Dictionary<string, AppInfo>? _apps;


    public RunApplicationPlugin(CurvaLauncherContext context) : base(context)
    {
        laziedIcon = new Lazy<ImageSource>(() => HostContext.ImageApi.CreateFromSvg(Resources.IconSvg)!);
    }

    public override void Initialize()
    {
        _apps = new();

        InitializeWin32();
        InitializeUwp();
    }

    private void InitializeAppsFolder()
    {
        var appxFactory = new AppxPackageHelper.AppxFactory();
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

            var name = Path.GetFileNameWithoutExtension(shortcut);

            _apps[name] = new Win32AppInfo()
            {
                Name = name,
                FilePath = target.FileName,
                Arguments = target.Arguments,
            };
        }
    }

    private void InitializeUwp()
    {
        if (_apps == null)
            return;

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

            UwpAppInfo info = new();

            if (subKey.GetValue("DisplayName") is not string displayName)
                continue;

            info.Name = displayName;

            if (subKey.GetValue("PackageID") is not string packageId)
                continue;

            info.PackageID = Regex.Replace(packageId, "_.*__", "_");

            if (subKey.GetValue("PackageRootFolder") is not string packageRootFolder)
                continue;

            info.PackageRootFolder = packageRootFolder;

            if (!System.IO.File.Exists(info.AppxManifestPath))
                continue;

            var appxManifestContent = File.ReadAllText(info.AppxManifestPath);

            XmlDocument xml = new();
            xml.LoadXml(appxManifestContent);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(xml.NameTable);//这一步实例化一个xml命名空间管理器
            nsManager.AddNamespace("ns", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
            nsManager.AddNamespace("uap", "http://schemas.microsoft.com/appx/manifest/uap/windows10");

            var appNode = xml.SelectSingleNode("/ns:Package/ns:Applications/ns:Application", nsManager);
            var logoNode = xml.SelectSingleNode("/ns:Package/ns:Properties/ns:Logo", nsManager);

            if (appNode == null || 
                appNode.Attributes?["Id"]?.Value is not string appId)
                continue;

            info.ApplicationId = appId;

            string? logoPath = logoNode?.InnerText;

            if (logoPath is not null)
            {
                var logoFullPath = Path.Combine(info.PackageRootFolder, logoPath);

                var logoFileName = Path.GetFileNameWithoutExtension(logoPath);
                var logoExtension = Path.GetExtension(logoPath);
                var logoFilesDir = Path.GetDirectoryName(logoFullPath) ?? ".";
                var logoFilesPattern = $"{logoFileName}*{logoExtension}";

                var regex = new Regex($@"{Regex.Escape(logoFileName)}\.scale-(?<scale>\d+){Regex.Escape(logoExtension)}");

                foreach (var logoFile in Directory.EnumerateFiles(logoFilesDir, logoFilesPattern))
                {

                }
            }

            _apps[info.Name] = info;
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
}
