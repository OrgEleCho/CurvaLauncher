using System.Diagnostics;
using System.Net.Cache;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.OpenUrl
{
    public class OpenUrlQueryResult : QueryResult
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

        public override float Weight => 1;

        public override string Title => $"{Url}";

        public override string Description => $"Open url in web browser: {Url}";

        public override ImageSource? Icon => icon;

        public override void Invoke()
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