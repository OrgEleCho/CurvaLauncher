

using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CurvaLauncher.Utilities;
using System.Text;

namespace CurvaLauncher.Plugin.RunProgram
{
    public class RunProgramQueryResult : ISyncQueryResult
    {
        public RunProgramQueryResult(CurvaLauncherContext context, string filename, string arguments)
        {
            FileName = filename;
            Arguments = arguments;

            context.Dispatcher.Invoke(() =>
            {
                icon = ImageUtils.GetFileIcon(filename, context.RequiredIconSize);
            });
        }

        private ImageSource? icon;

        public float Weight => 1;

        public string Title => $"{System.IO.Path.GetFileName(FileName)}";

        public string Description => !string.IsNullOrWhiteSpace(Arguments) ?
            $"Run Program: '{FileName}' with '{Arguments}'" :
            $"Run Program: '{FileName}'";

        public ImageSource? Icon => icon;

        public string FileName { get; }
        public string Arguments { get; }

        public void Invoke()
        {
            Process.Start(
                new ProcessStartInfo()
                {
                    FileName = FileName,
                    Arguments = Arguments,
                    UseShellExecute = true,
                });
        }
    }
}