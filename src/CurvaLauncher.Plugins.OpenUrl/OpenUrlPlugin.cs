using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Threading;
using CurvaLauncher.Plugins.OpenUrl.Properties;

namespace CurvaLauncher.Plugins.OpenUrl
{
    public class OpenUrlPlugin : SyncI18nPlugin
    {
        public static Regex UrlRegex { get; } 
            = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?");

        public override ImageSource Icon { get; }

        public override object NameKey => "StrPluginName";
        public override object DescriptionKey => "StrPluginDescription";

        public OpenUrlPlugin(CurvaLauncherContext context) : base(context)
        {
            Icon = context.ImageApi.CreateFromSvg(Resources.IconSvg)!;
        }


        public override IEnumerable<IQueryResult> Query(string query)
        {
            if (!UrlRegex.IsMatch(query))
                yield break;
            if (!Uri.TryCreate(query, UriKind.Absolute, out var uri))
                yield break;
            if (!uri.Scheme.Equals("http", StringComparison.OrdinalIgnoreCase) &&
                !uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                yield break;

            yield return new OpenUrlQueryResult(HostContext, uri);
        }

        public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
        {
            yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
        }
    }
}