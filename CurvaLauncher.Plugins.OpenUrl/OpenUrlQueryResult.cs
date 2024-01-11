using System.Diagnostics;
using System.Net.Cache;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CurvaLauncher.Plugin.OpenUrl
{
    public class OpenUrlQueryResult : ISyncQueryResult
    {
        public OpenUrlQueryResult(CurvaLauncherContext context, Uri url)
        {
            Url = url;

            context.Dispatcher.Invoke(() =>
            {
                icon = new BitmapImage(
                    new Uri(url, "favicon.ico"),
                    new RequestCachePolicy(RequestCacheLevel.CacheIfAvailable));
            });
        }

        private ImageSource? icon;

        public Uri Url { get; }

        public float Weight => 1;

        public string Title => $"{Url}";

        public string Description => $"Open url in web browser: {Url}";

        public ImageSource? Icon => icon;

        public void Invoke()
        {
            Process.Start(
                new ProcessStartInfo()
                {
                    FileName = Url.ToString(),
                    UseShellExecute = true,
                });
        }
    }
}