using System.IO;
using System.Windows.Media;
using System.Windows.Threading;
using CurvaLauncher.Utilities;
using CurvaLauncher.Data;
using Microsoft.Win32;

namespace CurvaLauncher.Plugin.RunProgram
{
    public class RunProgramPlugin : ISyncPlugin
    {
        public static string IconSvg { get; } =
            "<svg t=\"1695798431191\" class=\"icon\" viewBox=\"0 0 1024 1024\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" p-id=\"19522\" width=\"200\" height=\"200\"><path d=\"M64 833.05C64 853.37 80.13 870 99.84 870h824.32c19.71 0 35.84-16.63 35.84-36.95V297.2H64v535.85z\" fill=\"#94DBFF\" p-id=\"19523\"></path><path d=\"M341.76 726.8c-4.13 0-8.26-1.41-11.64-4.28L189.28 602.44c-5.6-4.78-8.8-11.64-8.8-18.84 0-7.19 3.2-14.05 8.8-18.83l140.84-120.09c7.51-6.4 18.83-5.52 25.27 1.99 6.42 7.53 5.53 18.82-1.99 25.24L222.41 583.6 353.4 695.28c7.52 6.42 8.42 17.72 1.99 25.24a17.878 17.878 0 0 1-13.63 6.28zM682.24 726.8c-5.06 0-10.08-2.13-13.63-6.28-6.42-7.53-5.53-18.82 1.99-25.24L801.59 583.6 670.6 471.92c-7.52-6.42-8.42-17.72-1.99-25.24 6.44-7.52 17.76-8.39 25.27-1.99l140.84 120.08c5.6 4.78 8.8 11.64 8.8 18.84 0 7.19-3.2 14.05-8.8 18.83L693.88 722.52a17.913 17.913 0 0 1-11.64 4.28zM431.34 741.02c-3.05 0-6.13-0.77-8.94-2.39-8.57-4.95-11.51-15.89-6.56-24.46l161.28-279.04c4.95-8.57 15.91-11.49 24.48-6.56 8.58 4.95 11.52 15.89 6.56 24.46L446.88 732.07c-3.32 5.74-9.34 8.95-15.54 8.95z\" fill=\"#FFFFFF\" p-id=\"19524\"></path><path d=\"M924.16 154H99.84C80.13 154 64 175.48 64 201.73v95.47h896v-95.47c0-26.25-16.13-47.73-35.84-47.73z\" fill=\"#6CB9FF\" p-id=\"19525\"></path><path d=\"M112.38 225.6a23.3 23.27 0 1 0 46.6 0 23.3 23.27 0 1 0-46.6 0Z\" fill=\"#FFFFFF\" p-id=\"19526\"></path><path d=\"M193.02 225.6a23.3 23.27 0 1 0 46.6 0 23.3 23.27 0 1 0-46.6 0Z\" fill=\"#FFFFFF\" p-id=\"19527\"></path><path d=\"M273.66 225.6a23.3 23.27 0 1 0 46.6 0 23.3 23.27 0 1 0-46.6 0Z\" fill=\"#FFFFFF\" p-id=\"19528\"></path></svg>";

        public string Name => "Run Program";
        public string Description => "Run programs under paths and registered apps";
        public ImageSource Icon => ImageUtils.CreateFromSvg(IconSvg);



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
            string name = query.ToLower();
            string nameExe = $"{name}.exe";

            string? path;
            if (_appPathes.TryGetValue(name, out path) ||
                _appPathes.TryGetValue(nameExe, out path))
                yield return new RunProgramQueryResult(context, path);
        }
    }
}