using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Threading;
using CurvaLauncher.Utilities;
using CurvaLauncher.Plugin.OpenUrl.Properties;

namespace CurvaLauncher.Plugin.OpenUrl
{
    public class OpenUrlPlugin : CurvaLauncherSyncPlugin
    {
        readonly Lazy<ImageSource> laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);

        public static Regex UrlRegex { get; } 
            = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?");

        public ImageSource Icon => laziedIcon.Value;

        public string Name => "Open URL";

        public string Description => "Use web browser to open URL";

        public void Initialize() { }
        public void Finish() { }


        public IEnumerable<IQueryResult> Query(CurvaLauncherContext context, string query)
        {
            if (!UrlRegex.IsMatch(query))
                yield break;
            if (!Uri.TryCreate(query, UriKind.Absolute, out var uri))
                yield break;
            if (!uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) &&
                !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                yield break;

            yield return new OpenUrlQueryResult(context, uri);
        }
    }
}