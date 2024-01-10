using System.Windows.Media;
using CurvaLauncher.Utilities;
using System.IO;
using CurvaLauncher.Plugin.RunApplication.Properties;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using System.Xml;

namespace CurvaLauncher.Plugin.RunApplication;

public class RunApplicationPlugin : ISyncPlugin
{
    readonly Lazy<ImageSource> laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);


    [PluginOption]
    public int ResultCount { get; set; } = 5;

    [PluginOption]
    public IndexLocations IndexLocations { get; set; } =
        IndexLocations.UWP |
        IndexLocations.CommonPrograms |
        IndexLocations.Programs |
        IndexLocations.Desktop;

    public string Name => "Run Application";
    public string Description => "Run Applications installed on your PC";
    public ImageSource Icon => laziedIcon.Value;

    static readonly int[] s_uwpIconSizes = [44, 150, 310];


    private Dictionary<string, AppInfo>? _apps;


    public RunApplicationPlugin()
    {

    }

    public void Initialize()
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
            if (FileUtils.GetShortcutTarget(shortcut) is not FileUtils.ShortcutTarget target)
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

            info.Name = subKey.GetValue("DisplayName")!.ToString()!;

            var PackageID = subKey.GetValue("PackageID")!.ToString()!;
            info.PackageID = Regex.Replace(PackageID, "_.*__", "_");

            info.PackageRootFolder = subKey.GetValue("PackageRootFolder")!.ToString()!;

            if (!File.Exists(info.AppxManifestPath))
                continue;

            var appxManifestContent = File.ReadAllText(info.AppxManifestPath);

            XmlDocument xml = new();
            xml.LoadXml(appxManifestContent);

            XmlNamespaceManager nsManager = new XmlNamespaceManager(xml.NameTable);//这一步实例化一个xml命名空间管理器
            nsManager.AddNamespace("ns", "http://schemas.microsoft.com/appx/manifest/foundation/windows10");
            nsManager.AddNamespace("uap", "http://schemas.microsoft.com/appx/manifest/uap/windows10");

            var appNode = xml.SelectSingleNode("/ns:Package/ns:Applications/ns:Application", nsManager);
            //var logoNode = xml.SelectSingleNode("/ns:Package/ns:Applications/ns:Application/uap:VisualElements", nsManager);

            if (appNode == null || 
                appNode.Attributes?["Id"]?.Value is not string appId)
                continue;

            info.ApplicationId = appId;

            //string? logoPath = logoNode?.Attributes?["Square150x150Logo"]?.Value;

            //if(logoPath is not null)
            //    info.LogoPath = Path.Combine(info.PackageRootFolder, logoPath);

            _apps[info.Name] = info;
        }

    }

    public void Finish()
    {
        _apps = null;
    }

    public IEnumerable<IQueryResult> Query(CurvaLauncherContext context, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            yield break;
        if (_apps == null)
            yield break;

        var results = _apps
            .Select(kv => (kv.Key, kv.Value, Weight: StringUtils.Match(kv.Key.ToLower(), query.ToLower())))
            .OrderByDescending(kvw => kvw.Weight)
            .Take(ResultCount);

        foreach (var result in results)
        {
            if (result.Value is Win32AppInfo win32AppInfo)
                yield return new RunWin32ApplicationQueryResult(context, win32AppInfo, result.Weight);
            else if (result.Value is UwpAppInfo uwpAppInfo)
                yield return new RunUwpApplicationQueryResult(context, uwpAppInfo, result.Weight);
        }
    }
}
