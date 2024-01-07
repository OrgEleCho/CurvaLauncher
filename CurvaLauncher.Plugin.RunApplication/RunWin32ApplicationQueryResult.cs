using System.Windows.Media;
using CurvaLauncher.Utilities;
using CurvaLauncher.Data;
using System.Diagnostics;
using System.IO;

namespace CurvaLauncher.Plugin.RunApplication
{
    public class RunWin32ApplicationQueryResult : SyncQueryResult
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
                    WorkingDirectory = Path.GetDirectoryName(FileName),
                    UseShellExecute = true,
                });
        }
    }
}