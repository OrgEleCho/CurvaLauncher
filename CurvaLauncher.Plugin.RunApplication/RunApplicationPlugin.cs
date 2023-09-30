using System.Windows.Media;
using CurvaLauncher.Utilities;
using CurvaLauncher.Data;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace CurvaLauncher.Plugin.RunApplication
{
    public class RunApplicationPlugin : ISyncPlugin
    {
        public static string IconSvg { get; }
            = "<svg t=\"1695798514906\" class=\"icon\" viewBox=\"0 0 1024 1024\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" p-id=\"22357\" width=\"128\" height=\"128\"><path d=\"M471.186286 25.453714a86.820571 86.820571 0 0 1 81.627428 0l368.054857 196.022857c28.306286 15.067429 46.006857 44.544 46.006858 76.653715v417.865143c0 31.451429-17.042286 60.562286-44.617143 75.849142l-367.908572 205.385143c-26.331429 14.628571-58.368 14.628571-84.699428 0l-367.908572-205.385143A86.893714 86.893714 0 0 1 57.051429 716.068571V298.130286c0-32.182857 17.700571-61.586286 46.08-76.726857zM512 97.499429l-2.194286 0.512-367.908571 196.022857a4.608 4.608 0 0 0-2.486857 4.096v417.865143c0 1.682286 0.950857 3.218286 2.340571 4.022857L509.805714 925.257143c1.316571 0.731429 3.072 0.731429 4.388572 0l368.054857-205.312a4.608 4.608 0 0 0 2.340571-4.022857V298.130286a4.608 4.608 0 0 0-2.413714-4.096L514.121143 98.011429a4.534857 4.534857 0 0 0-4.242286 0z m276.114286 258.925714a36.571429 36.571429 0 0 1-12.580572 49.737143l-0.512 0.292571L548.571429 538.916571l0.073142 259.949715a36.571429 36.571429 0 0 1-73.142857 0.585143V540.672L246.564571 406.381714a36.571429 36.571429 0 0 1 36.498286-63.341714l0.512 0.292571 226.742857 133.12 227.693715-133.12a36.571429 36.571429 0 0 1 50.029714 13.092572z\" fill=\"#3AA5FF\" p-id=\"22358\"></path></svg>";

        public ImageSource Icon => ImageUtils.CreateFromSvg(IconSvg);

        [PluginOption]
        public int ResultCount { get; set; } = 5;

        public string Name => "Run Application";

        public string Description => "Run Applications installed on your PC";

        private readonly Dictionary<string, string> _win32appPathes = new Dictionary<string, string>();

        public RunApplicationPlugin()
        {

        }

        public void Init()
        {
            var commonProgramsFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
            var programsFolder = Environment.GetFolderPath(Environment.SpecialFolder.Programs);

            var allShotcutsInStartMenu = new List<string>();

            allShotcutsInStartMenu.AddRange(Directory.GetFiles(commonProgramsFolder, "*.lnk", SearchOption.AllDirectories));
            allShotcutsInStartMenu.AddRange(Directory.GetFiles(programsFolder, "*.lnk", SearchOption.AllDirectories));

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

        public IEnumerable<QueryResult> Query(CurvaLauncherContext context, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
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