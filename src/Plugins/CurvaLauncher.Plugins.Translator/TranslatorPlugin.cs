using System.Globalization;
using System.Net.Http;
using System.Windows.Media;
using CurvaLauncher.Apis;
using CurvaLauncher.Plugins.Translator.Properties;

namespace CurvaLauncher.Plugins.Translator
{

    public class TranslatorPlugin : CommandSyncI18nPlugin
    {
        [PluginI18nOption("StrCommandName")]
        public string CommandName { get; set; } = "Trans";

        [PluginI18nOption("StrTranslatorAPI")]
        public TranslatorAPI TranslatorAPI { get; set; } = TranslatorAPI.Youdao;

        [PluginI18nOption("StrSourceLanguage")]
        public string SourceLanguage { get; set; } = "Auto";

        [PluginI18nOption("StrTargetLanguage")]
        public string TargetLanguage { get; set; } = "Auto";

        [PluginI18nOption("StrPlaceholderForEmptyResult")]
        public string PlaceholderForEmptyResult { get; set; } = "[None]";


        public override IEnumerable<string> CommandNames
        {
            get
            {
                yield return CommandName;
            }
        }


        internal HttpClient? _httpClient;

        public override ImageSource Icon { get; }

        public override object NameKey => "StrPluginName";

        public override object DescriptionKey => "StrPluginDescription";

        public TranslatorPlugin(CurvaLauncherContext context) : base(context)
        {
            Icon = context.ImageApi.CreateFromSvg(Resources.IconSvg)!;
        }

        public override void Initialize()
        {
            base.Initialize();

            _httpClient = new();
        }

        public override void Finish()
        {
            base.Finish();

            _httpClient = null;
        }

        public override IEnumerable<IQueryResult> ExecuteCommand(string commandName, CommandLineSegment[] arguments)
        {
            if (!Enum.IsDefined<TranslatorAPI>(TranslatorAPI))
                yield break;
            if (_httpClient == null)
                yield break;

            if (arguments.Length == 0)
            {
                yield return new EmptyQueryResult("Translate text", "Input something to get translation", 1, null);
                yield break;
            }

            string text = HostContext.CommandLineApi.Concat(arguments);

            string? sourceLanguage = SourceLanguage;
            string? targetLanguage = TargetLanguage;

            if ("Auto".Equals(sourceLanguage, StringComparison.OrdinalIgnoreCase))
                sourceLanguage = null;
            if ("Auto".Equals(targetLanguage, StringComparison.OrdinalIgnoreCase))
                targetLanguage = null;

            yield return TranslatorAPI switch
            {
                TranslatorAPI.Youdao => new Youdao.YoudaoTranslationQueryResult(this, _httpClient, sourceLanguage, targetLanguage, text),
                TranslatorAPI.MicrosoftEdge => new MicrosoftEdge.EdgeTranslationQueryResult(HostContext, _httpClient, sourceLanguage, targetLanguage, text),

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
    }
}

