using System.Globalization;
using System.Windows.Media;
using CurvaLauncher.Apis;
using CurvaLauncher.Plugins.QuickWebSearch.Properties;

namespace CurvaLauncher.Plugins.QuickWebSearch
{
    public class QuickWebSearchPlugin : CommandSyncI18nPlugin
    {
        readonly Lazy<ImageSource> _laziedIcon;
        readonly ImageSource _baiduIcon;
        readonly ImageSource _bingIcon;
        readonly ImageSource _googleIcon;
        readonly ImageSource _duckIcon;

        [PluginI18nOption("StrCommandNameForBaidu")]
        public string CommandNameBaidu { get; set; } = "Baidu";

        [PluginI18nOption("StrCommandNameForBing")]
        public string CommandNameBing { get; set; } = "Bing";

        [PluginI18nOption("StrCommandNameForGoogle")]
        public string CommandNameGoogle { get; set; } = "Google";

        [PluginI18nOption("StrCommandNameForDuckDuckGo")]
        public string CommandNameDuckDuckGo { get; set; } = "Duck";


        public override ImageSource Icon => _laziedIcon.Value;
        public override IEnumerable<string> CommandNames
        {
            get
            {
                yield return CommandNameBaidu;
                yield return CommandNameBing;
                yield return CommandNameGoogle;
                yield return CommandNameDuckDuckGo;
            }
        }

        public override object NameKey => "StrPluginName";
        public override object DescriptionKey => "StrPluginDescription";

        public QuickWebSearchPlugin(CurvaLauncherContext context) : base(context)
        {
            _laziedIcon = new Lazy<ImageSource>(() => context.ImageApi.CreateFromSvg(Resources.IconSvg)!);
            _baiduIcon = context.ImageApi.CreateFromSvg(Resources.IconBaiduSvg)!;
            _bingIcon = context.ImageApi.CreateFromSvg(Resources.IconBingSvg)!;
            _googleIcon = context.ImageApi.CreateFromSvg(Resources.IconGoogleSvg)!;
            _duckIcon = context.ImageApi.CreateFromSvg(Resources.IconDuckSvg)!;
        }

        private SearchEngine GetSearchEngineFromCommand(string commandName)
        {
            var stringComparison = IgnoreCases ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

            if (commandName.Equals(CommandNameBaidu, stringComparison))
                return SearchEngine.Baidu;
            if (commandName.Equals(CommandNameBing, stringComparison))
                return SearchEngine.Bing;
            if (commandName.Equals(CommandNameGoogle, stringComparison))
                return SearchEngine.Google;
            if (commandName.Equals(CommandNameDuckDuckGo, stringComparison))
                return SearchEngine.DuckDuckGo;

            return (SearchEngine)(-1);
        }

        private ImageSource? GetSearchEngineIcon(SearchEngine searchEngine)
        {
            return searchEngine switch
            {
                SearchEngine.Baidu => _baiduIcon,
                SearchEngine.Bing => _bingIcon,
                SearchEngine.Google => _googleIcon,
                SearchEngine.DuckDuckGo => _duckIcon,
                _ => null
            };
        }

        public override IEnumerable<IQueryResult> ExecuteCommand(string commandName, CommandLineSegment[] arguments)
        {
            var searchEngine = GetSearchEngineFromCommand(commandName);
            var icon = GetSearchEngineIcon(searchEngine);

            if (!Enum.IsDefined(searchEngine))
                yield break;

            if (arguments.Length == 0)
            {
                yield return new EmptyQueryResult("Quick Web Search", "Type something to search the web", 1, icon);
                yield break;
            }

            string kwd = HostContext.CommandLineApi.Concat(arguments);
            string escapedKwd = Uri.EscapeDataString(kwd);
            yield return searchEngine switch
            {
                SearchEngine.Baidu => new QuickWebSearchQueryResult(HostContext, "Baidu", kwd, $"https://www.baidu.com/s?wd={escapedKwd}", icon),
                SearchEngine.Bing => new QuickWebSearchQueryResult(HostContext, "Bing", kwd, $"https://www.bing.com/search?q={escapedKwd}", icon),
                SearchEngine.Google => new QuickWebSearchQueryResult(HostContext, "Google", kwd, $"https://www.google.com/search?q={escapedKwd}", icon),
                SearchEngine.DuckDuckGo => new QuickWebSearchQueryResult(HostContext, "DuckDuckGo", kwd, $"https://duckduckgo.com/?q={escapedKwd}", icon),
                _ => throw new Exception("This would never happen")
            };
        }

        public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
        {
            yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("de"), "I18n/De.xaml");
        }

        enum SearchEngine
        {
            Baidu, Bing, Google, DuckDuckGo
        }
    }
}
