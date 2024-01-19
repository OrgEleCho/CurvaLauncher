using System.IO;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Win32;
using CurvaLauncher.Plugins.RunProgram.Properties;
using System.Text;
using CurvaLauncher.Apis;
using System.Globalization;

namespace CurvaLauncher.Plugins.RunProgram
{
    public class RunProgramPlugin : SyncI18nPlugin
    {
        [PluginI18nOption("StrIncludeDirectories", AllowTextMultiline = true)]
        public string IncludeDirectories { get; set; } = string.Empty;

        [PluginI18nOption("StrExcludeDirectories", AllowTextMultiline = true)]
        public string ExcludeDirectories { get; set; } = string.Empty;

        [PluginI18nOption("StrEnableAppsIndexing", AllowTextMultiline = true)]
        public bool EnableAppsIndexing { get; set; } = true;


        public override ImageSource Icon { get; }

        public override object NameKey => "StrPluginName";
        public override object DescriptionKey => "StrPluginDescription";


        public RunProgramPlugin(CurvaLauncherContext context) : base(context)
        {
            Icon = context.ImageApi.CreateFromSvg(Resources.IconSvg)!;
        }

        Dictionary<string, string>? _appPathes;

        public override void Initialize()
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

        public override void Finish()
        {
            _appPathes = null;
        }

        public override IEnumerable<IQueryResult> Query(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                yield break;
            if (_appPathes == null)
                yield break;

            HostContext.CommandLineApi.Split(query, out var segments);

            string name = segments[0].Value;
            string nameExe = $"{name}.exe";

            string? path;
            if (_appPathes.TryGetValue(name, out path) ||
                _appPathes.TryGetValue(nameExe, out path))
            {
                string arguments = HostContext.CommandLineApi.Concat(segments.Skip(1));

                yield return new RunProgramQueryResult(HostContext, path, arguments);
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
    }
}