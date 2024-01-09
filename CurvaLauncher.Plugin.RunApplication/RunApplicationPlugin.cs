using System.Windows.Media;
using CurvaLauncher.Utilities;
using System.IO;
using System.ComponentModel.DataAnnotations;
using CurvaLauncher.Plugin.RunApplication.Properties;

namespace CurvaLauncher.Plugin.RunApplication
{
    public class RunApplicationPlugin : ISyncPlugin
    {
        readonly Lazy<ImageSource> laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);


        [PluginOption]
        public int ResultCount { get; set; } = 5;

        [PluginOption]
        public IndexLocations IndexLocations { get; set; } =
            IndexLocations.CommonPrograms |
            IndexLocations.Programs |
            IndexLocations.Desktop;

        public string Name => "Run Application";
        public string Description => "Run Applications installed on your PC";
        public ImageSource Icon => laziedIcon.Value;


        private Dictionary<string, string>? _win32appPathes;

        public RunApplicationPlugin()
        {

        }

        public void Initialize()
        {
            _win32appPathes = new();

            var allShotcutsInStartMenu = new List<string>();

            allShotcutsInStartMenu.AddRange(
                Directory.GetFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), "*.lnk", SearchOption.AllDirectories));
            allShotcutsInStartMenu.AddRange(
                Directory.GetFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.Programs), "*.lnk", SearchOption.AllDirectories));
            allShotcutsInStartMenu.AddRange(
                Directory.GetFiles(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "*.lnk", SearchOption.TopDirectoryOnly));

            foreach (var shortcut in allShotcutsInStartMenu)
            {
                if (FileUtils.GetShortcutTarget(shortcut) is not string target)
                    continue;
                var ext = Path.GetExtension(target);
                if (!string.Equals(ext, ".exe", StringComparison.OrdinalIgnoreCase))
                    continue;

                var name = Path.GetFileNameWithoutExtension(shortcut);

                _win32appPathes[name] = target;
            }
        }

        public void Finish()
        {
            _win32appPathes = null;
        }

        public IEnumerable<IQueryResult> Query(CurvaLauncherContext context, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                yield break;
            if (_win32appPathes == null)
                yield break;

            var results = _win32appPathes
                .Select(kv => (kv.Key, kv.Value, Weight: StringUtils.Match(kv.Key.ToLower(), query.ToLower())))
                .OrderByDescending(kvw => kvw.Weight)
                .Take(ResultCount);

            foreach (var result in results)
                yield return new RunWin32ApplicationQueryResult(context, result.Key, result.Value, result.Weight);
        }
    }
}