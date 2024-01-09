using System.Windows.Media;
using CurvaLauncher.Utilities;
using System.Diagnostics;
using System.IO;

namespace CurvaLauncher.Plugin.RunApplication
{
    public class RunWin32ApplicationQueryResult : ISyncQueryResult
    {
        public RunWin32ApplicationQueryResult(CurvaLauncherContext context, string appName, string filename, float weight)
        {
            AppName = appName;
            FileName = filename;
            Weight = weight;

            context.Dispatcher.Invoke(() =>
            {
                icon = ImageUtils.GetFileIcon(filename, context.RequiredIconSize);
            });
        }

        private ImageSource? icon;

        public float Weight { get; }

        public string Title => $"{AppName}";

        public string Description => $"Run Application: {FileName}";

        public ImageSource? Icon => icon;

        public string AppName { get; }
        public string FileName { get; }

        public void Invoke()
        {
            Process.Start(
                new ProcessStartInfo()
                {
                    FileName = FileName,
                    WorkingDirectory = Path.GetDirectoryName(FileName),
                    UseShellExecute = true,
                });
        }
    }
}