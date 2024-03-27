using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using EverythingSearchClient;

namespace CurvaLauncher.Plugins.Everything
{
    public class EverythingPlugin : SyncI18nPlugin
    {
        public EverythingPlugin(CurvaLauncherContext context) : base(context)
        {
            Icon = null!;
        }

        public override object NameKey => "StrPluginName";
        public override object DescriptionKey => "StrPluginDescription";
        public override ImageSource Icon { get; }


        [PluginI18nOption("StrPrefix")]
        public string Prefix { get; set; } = "*";

        [PluginI18nOption("StrResultMaxCount")]
        public int ResultMaxCount { get; set; } = 10;

        public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
        {
            yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("de"), "I18n/De.xaml");
        }

        public override IEnumerable<IQueryResult> Query(string query)
        {
            if (!query.StartsWith(Prefix))
                yield break;
            query = query.Substring(Prefix.Length);

            if (!SearchClient.IsEverythingAvailable())
            {
                yield return new EverythingNotRunningQueryResult();
                yield break;
            }

            if (string.IsNullOrWhiteSpace(query))
            {
                yield return new EmptyQueryResult("Everything", "Locate files and folders by name instantly.", 1, null);
                yield break;
            }

            var client = new SearchClient();
            var result = client.Search(query, (uint)ResultMaxCount);

            float weight = 1;
            foreach (var item in result.Items)
            {
                yield return new EverythingQueryResult(this, item, weight);
                weight -= float.Epsilon;
            }
        }
    }
}
