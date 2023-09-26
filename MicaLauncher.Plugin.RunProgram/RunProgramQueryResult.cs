

using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MicaLauncher.Common;
using MicaLauncher.Common.Utilities;
using MicaLauncher.Data;

namespace MicaLauncher.Plugin.RunProgram
{
    public class RunProgramQueryResult : QueryResult
    {
        public RunProgramQueryResult(MicaLauncherContext context, string filename)
        {
            FileName = filename;

            context.Dispatcher.Invoke(() =>
            {
                icon = ImageUtils.GetFileIcon(filename, context.RequiredIconSize);
            });
        }

        private ImageSource? icon;

        public override float Match => 1;

        public override string Title => $"Run {System.IO.Path.GetFileName(FileName)}";

        public override string Description => $"Run {FileName}";

        public override ImageSource? Icon => icon;

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