

using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CurvaLauncher.Utilities;
using CurvaLauncher.Data;
using System.Text;

namespace CurvaLauncher.Plugin.RunProgram
{
    public class RunProgramQueryResult : SyncQueryResult
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

        public override float Weight => 1;

        public override string Title => $"{System.IO.Path.GetFileName(FileName)}";

        public override string Description => !string.IsNullOrWhiteSpace(Arguments) ?
            $"Run Program: '{FileName}' with '{Arguments}'" :
            $"Run Program: '{FileName}'";

        public override ImageSource? Icon => icon;

        public string FileName { get; }
        public string Arguments { get; }

        public override void Invoke()
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