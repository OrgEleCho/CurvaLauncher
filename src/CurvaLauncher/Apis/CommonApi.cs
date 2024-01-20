using System.Diagnostics;
using System.Windows.Media;
using CurvaLauncher.Views.Dialogs;

namespace CurvaLauncher.Apis
{
    public class CommonApi : ICommonApi
    {
        private CommonApi() { }

        public static CommonApi Instance { get; } = new();

        public void Open(string name) => Process.Start(
            new ProcessStartInfo()
            {
                FileName = name,
                UseShellExecute = true,
            });

        public void OpenExecutable(string file) => Process.Start(
            new ProcessStartInfo()
            {
                FileName = file,
                UseShellExecute = false
            });

        public void ShowImage(ImageSource image, ImageOptions options)
        {
            new SimpleImageDialog(image, options)
                .Show();
        }

        public void ShowText(string text, TextOptions options)
        {
            new SimpleTextDialog(text, options)
                .Show();
        }
    }
}
