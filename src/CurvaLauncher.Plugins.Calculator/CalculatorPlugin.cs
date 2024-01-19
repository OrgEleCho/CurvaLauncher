using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using CurvaLauncher.Plugins.Calculator.Properties;
using NCalculatorLib;

namespace CurvaLauncher.Plugins.Calculator
{

    public class CalculatorPlugin : SyncI18nPlugin
    {
        [PluginI18nOption("StrPrefix")]
        public string Prefix { get; set; } = "=";

        public override ImageSource Icon { get; }

        public override object NameKey => "StrPluginName";
        public override object DescriptionKey => "StrPluginDescription";

        public CalculatorPlugin(CurvaLauncherContext context) : base(context)
        {
            Icon = context.ImageApi.CreateFromSvg(Resources.IconSvg)!;
        }

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

            string expr = query.Substring(Prefix.Length);

            if (NCalc.TryEval(expr, out var result))
                yield return new CalculatorQueryResult(result);
        }
    }
}