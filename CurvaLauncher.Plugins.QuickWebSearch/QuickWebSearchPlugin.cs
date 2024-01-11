using System.Windows.Media;
using CurvaLauncher.Apis;
using CurvaLauncher.Plugins.QuickWebSearch.Properties;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Plugins.QuickWebSearch
{
    public class QuickWebSearchPlugin : CurvaLauncherSyncCommandPlugin
    {
        Lazy<ImageSource> _laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);

        ImageSource _baiduIcon = ImageUtils.CreateFromSvg(Resources.IconBaiduSvg)!;
        ImageSource _bingIcon = ImageUtils.CreateFromSvg(Resources.IconBingSvg)!;
        ImageSource _googleIcon = ImageUtils.CreateFromSvg(Resources.IconGoogleSvg)!;
        ImageSource _duckIcon = ImageUtils.CreateFromSvg(Resources.IconDuckSvg)!;


        [PluginOption("Command name for Baidu")]
        public string CommandNameBaidu { get; set; } = "Baidu";

        [PluginOption("Command name for Bing")]
        public string CommandNameBing { get; set; } = "Bing";

        [PluginOption("Command name for Google")]
        public string CommandNameGoogle { get; set; } = "Google";

        [PluginOption("Command name for DuckDuckGo")]
        public string CommandNameDuckDuckGo { get; set; } = "Duck";



        public override string Name => "Quick Web Search";

        public override string Description => "Quick search the web for something";

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
            return SearchEngine.Unknown;
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

        public override IEnumerable<IQueryResult> ExecuteCommand(CurvaLauncherContext context, string commandName, CommandLineSegment[] arguments)
        {
            var searchEngine = GetSearchEngineFromCommand(commandName);
            var icon = GetSearchEngineIcon(searchEngine);

            if (searchEngine == SearchEngine.Unknown)
                yield break;

            if (arguments.Length == 0)
            {
                yield return new EmptyQueryResult("Quick Web Search", "Type something to search the web", 1, icon);
                yield break;
            }

            string kwd = CommandLineUtils.Concat(arguments);
            string escapedKwd = Uri.EscapeDataString(kwd);
            yield return searchEngine switch
            {
                SearchEngine.Baidu => new QuickWebSearchQueryResult("Baidu", kwd, $"https://www.baidu.com/s?wd={escapedKwd}", icon),
                SearchEngine.Bing => new QuickWebSearchQueryResult("Bing", kwd, $"https://www.bing.com/search?q={escapedKwd}", icon),
                SearchEngine.Google => new QuickWebSearchQueryResult("Google", kwd, $"https://www.google.com/search?q={escapedKwd}", icon),
                SearchEngine.DuckDuckGo => new QuickWebSearchQueryResult("DuckDuckGo", kwd, $"https://duckduckgo.com/?q={escapedKwd}", icon),
                _ => throw new Exception("This would never happen")
            };
        }

        enum SearchEngine
        {
            Unknown,
            Baidu, Bing, Google, DuckDuckGo
        }
    }
}
