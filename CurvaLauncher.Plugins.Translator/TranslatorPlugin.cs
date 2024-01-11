using System.Windows.Media;
using CurvaLauncher.Apis;
using CurvaLauncher.Plugin.Translator.Properties;

namespace CurvaLauncher.Plugin.Translator
{

    public class TranslatorPlugin : CurvaLauncherSyncCommandPlugin
    {
        [PluginOption]
        public string CommandName { get; set; } = "Trans";

        [PluginOption]
        public TranslatorAPI TranslatorAPI { get; set; } = TranslatorAPI.Youdao;

        [PluginOption]
        public string SourceLanguage { get; set; } = "Auto";

        [PluginOption]
        public string TargetLanguage { get; set; } = "Auto";


        public override IEnumerable<string> CommandNames
        {
            get
            {
                yield return CommandName;
            }
        }


        HttpClient? _httpClient;
        readonly Lazy<ImageSource> _laziedIcon;

        public override string Name => "Translator";

        public override string Description => "Translate texts with free APIs";

        public override ImageSource Icon => _laziedIcon.Value;

        public TranslatorPlugin(CurvaLauncherContext context) : base(context)
        {
            _laziedIcon = new Lazy<ImageSource>(() => context.ImageApi.CreateFromSvg(Resources.IconSvg)!);
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

        public override IEnumerable<IQueryResult> ExecuteCommand(CurvaLauncherContext context, string commandName, CommandLineSegment[] arguments)
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

            string text = context.CommandLineApi.Concat(arguments);

            string? sourceLanguage = SourceLanguage;
            string? targetLanguage = TargetLanguage;

            if ("Auto".Equals(sourceLanguage, StringComparison.OrdinalIgnoreCase))
                sourceLanguage = null;
            if ("Auto".Equals(targetLanguage, StringComparison.OrdinalIgnoreCase))
                targetLanguage = null;

            yield return TranslatorAPI switch
            {
                TranslatorAPI.Youdao => new Youdao.YoudaoTranslationQueryResult(_httpClient, sourceLanguage, targetLanguage, text),
                TranslatorAPI.MicrosoftEdge => new MicrosoftEdge.EdgeTranslationQueryResult(_httpClient, sourceLanguage, targetLanguage, text),

                _ => throw new Exception("This would never happen")
            };
        }
    }
}

