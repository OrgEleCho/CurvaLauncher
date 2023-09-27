using System.Windows.Media;
using MicaLauncher.Utilities;
using MicaLauncher.Data;
using System.Diagnostics;

namespace MicaLauncher.Plugin.RunApplication
{
    public class RunWin32ApplicationQueryResult : QueryResult
    {
        public RunWin32ApplicationQueryResult(MicaLauncherContext context, string appName, string filename, float weight)
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

        public override float Weight { get; }

        public override string Title => $"{AppName}";

        public override string Description => $"Run Application: {FileName}";

        public override ImageSource? Icon => icon;

        public string AppName { get; }
        public string FileName { get; }

        public override void Invoke()
        {
            Process.Start(
                new ProcessStartInfo()
                {
                    FileName = FileName,
                    UseShellExecute = true,
                });
        }
    }
}