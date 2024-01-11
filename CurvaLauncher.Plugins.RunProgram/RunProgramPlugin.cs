using System.IO;
using System.Windows.Media;
using System.Windows.Threading;
using CurvaLauncher.Utilities;
using Microsoft.Win32;
using CurvaLauncher.Plugins.RunProgram.Properties;
using System.Text;
using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugins.RunProgram
{
    public class RunProgramPlugin : CurvaLauncherSyncPlugin
    {
        readonly Lazy<ImageSource> _laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);

        [PluginOption(AllowTextMultiline = true)]
        public string IncludeDirectories { get; set; } = string.Empty;

        [PluginOption(AllowTextMultiline = true)]
        public string ExcludeDirectories { get; set; } = string.Empty;

        [PluginOption]
        public bool EnableAppsIndexing { get; set; } = true;

        public string Name => "Run Program";
        public string Description => "Run programs under paths and registered apps";
        public ImageSource Icon => _laziedIcon.Value;



        Dictionary<string, string>? _appPathes;

        public void Initialize()
        {
            _appPathes = new();

            HashSet<string> includeDirectories = IncludeDirectories
                .Split(['\r', '\n'])
                .ToHashSet();
            HashSet<string> excludeDirectories = ExcludeDirectories
                .Split(['\r', '\n'])
                .ToHashSet();

            string? pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (pathVariable != null)
            {
                string[] directories = pathVariable.Split(';');
                foreach (string directory in directories.Concat(includeDirectories))
                {
                    if (!Directory.Exists(directory))
                        continue;
                    if (excludeDirectories.Contains(directory))
                        continue;

                    foreach (var file in Directory.GetFiles(directory))
                    {
                        var name = Path.GetFileName(file).ToLower();
                        _appPathes[name] = file;
                    }
                }
            }

            if (EnableAppsIndexing)
            {
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
        }

        public void Finish()
        {
            _appPathes = null;
        }

        public IEnumerable<IQueryResult> Query(CurvaLauncherContext context, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                yield break;
            if (_appPathes == null)
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