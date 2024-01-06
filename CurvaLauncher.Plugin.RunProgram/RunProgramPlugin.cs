using System.IO;
using System.Windows.Media;
using System.Windows.Threading;
using CurvaLauncher.Utilities;
using CurvaLauncher.Data;
using Microsoft.Win32;
using CurvaLauncher.Plugin.RunProgram.Properties;
using System.Text;

namespace CurvaLauncher.Plugin.RunProgram
{
    public class RunProgramPlugin : ISyncPlugin
    {
        readonly Lazy<ImageSource> laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);


        public string Name => "Run Program";
        public string Description => "Run programs under paths and registered apps";
        public ImageSource Icon => laziedIcon.Value;



        Dictionary<string, string> _appPathes = new Dictionary<string, string>();

        public void Init()
        {
            string? pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (pathVariable != null)
            {
                string[] directories = pathVariable.Split(';');
                foreach (string directory in directories)
                {
                    if (!Directory.Exists(directory))
                        continue;

                    foreach (var file in Directory.GetFiles(directory))
                    {
                        var name = Path.GetFileName(file).ToLower();
                        _appPathes[name] = file;
                    }
                }
            }

            RegistryKey? registryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths", false);
            if (registryKey != null)
            {
                foreach (var subkeyName in registryKey.GetSubKeyNames())
                {
                    var subkey = registryKey.OpenSubKey(subkeyName, false);

                    if (subkey == null || subkey.GetValue(null) is not string appPath)
                        continue;

                    string name = subkeyName.ToLower();
                    _appPathes[name] = appPath;
                }
            }
        }

        public IEnumerable<QueryResult> Query(CurvaLauncherContext context, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                yield break;

            CommandLineUtils.Split(query, out var segments);

            string name = segments[0].Value;
            string nameExe = $"{name}.exe";

            string? path;
            if (_appPathes.TryGetValue(name, out path) ||
                _appPathes.TryGetValue(nameExe, out path))
            {
                string arguments = CommandLineUtils.Concat(segments.Skip(1));

                yield return new RunProgramQueryResult(context, path, arguments);
            }
        }
    }
}