using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using CurvaLauncher;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Plugin.RunApplication
{
    public class RunWin32ApplicationQueryResult : IAsyncQueryResult
    {
        public RunWin32ApplicationQueryResult(CurvaLauncherContext context, string appName, string filename, string? commandLineArguments, float weight)
        {
            AppName = appName;
            FileName = filename;
            CommandLineArguments = commandLineArguments;
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
        public string? CommandLineArguments { get; }

        public async Task InvokeAsync(CancellationToken cancellationToken)
        {
            var process = Process.Start(
                new ProcessStartInfo()
                {
                    FileName = FileName,
                    Arguments = CommandLineArguments,
                    WorkingDirectory = Path.GetDirectoryName(FileName),
                    UseShellExecute = true,
                });

            if (process == null)
                return;

            Task inputWait = Task.Run(process.WaitForInputIdle, cancellationToken);

            while (process.MainWindowHandle == IntPtr.Zero)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                if (process.HasExited)
                    break;
                
                if (inputWait.IsCompleted)
                    break;

                await Task.Delay(1);
            }
        }
    }
}