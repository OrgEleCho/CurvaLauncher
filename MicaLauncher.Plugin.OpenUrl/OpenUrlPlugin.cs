using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Threading;
using MicaLauncher.Common;
using MicaLauncher.Data;

namespace MicaLauncher.Plugin.OpenUrl
{
    public class OpenUrlPlugin : IPlugin
    {
        public static Regex UrlRegex { get; } 
            = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?");

        public ImageSource Icon => null!;

        public IEnumerable<QueryResult> Query(MicaLauncherContext context, string query)
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