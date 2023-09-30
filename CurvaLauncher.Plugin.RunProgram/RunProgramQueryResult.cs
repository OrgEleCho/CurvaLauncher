

using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CurvaLauncher.Utilities;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.RunProgram
{
    public class RunProgramQueryResult : QueryResult
    {
        public RunProgramQueryResult(CurvaLauncherContext context, string filename)
        {
            FileName = filename;

            context.Dispatcher.Invoke(() =>
            {
                icon = ImageUtils.GetFileIcon(filename, context.RequiredIconSize);
            });
        }

        private ImageSource? icon;

        public override float Weight => 1;

        public override string Title => $"{System.IO.Path.GetFileName(FileName)}";

        public override string Description => $"Run Program: {FileName}";

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