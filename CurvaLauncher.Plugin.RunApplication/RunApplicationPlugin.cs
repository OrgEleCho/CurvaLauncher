using System.Windows.Media;
using CurvaLauncher.Utilities;
using System.IO;
using CurvaLauncher.Plugin.RunApplication.Properties;
using Microsoft.Win32;

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


    private Dictionary<string, AppInfo>? _apps;

    
    public RunApplicationPlugin()
    {

    }

    public void Initialize()
    {
        _apps = new();

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

            _apps[name] = target;
        }

        InitializeUwp();
    }

    public void InitializeUwp()
    {
        var repositoryKey = Registry.CurrentUser.OpenSubKey(
            @"Software\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages");

        if (repositoryKey is null)
            return;

        var subKeys = repositoryKey.GetSubKeyNames();

        foreach (var item in subKeys.Where(v => v.Contains("Minecraft")))
        {
            Console.WriteLine(item);
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
                yield return new RunWin32ApplicationQueryResult(context, result.Key, result.Value.FileName, result.Value.CommandLineArguments, result.Weight);
        }
    }
}