using System.Windows.Media;
using CurvaLauncher.Data;
using CurvaLauncher.Plugin.Translator.Properties;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Plugin.Translator
{

    public class TranslatorPlugin : SyncCommandPlugin
    {
        [PluginOption]
        public string CommandName { get; set; } = "Trans";

        [PluginOption]
        public TranslatorAPI TranslatorAPI { get; set; } = TranslatorAPI.Youdao;


        public override IEnumerable<string> CommandNames
        {
            get
            {
                yield return CommandName;
            }
        }

        Lazy<ImageSource> _laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);

        public override string Name => "Translator";

        public override string Description => "Translate texts with free APIs";

        public override ImageSource Icon => _laziedIcon.Value;

        public HttpClient HttpClient { get; private set; }

        public TranslatorPlugin()
        {
            HttpClient = new();
        }

        public override IEnumerable<QueryResult> ExecuteCommand(CurvaLauncherContext context, string commandName, CommandLineSegment[] arguments)
        {
            if (!Enum.IsDefined<TranslatorAPI>(TranslatorAPI))
                yield break;

            if (arguments.Length == 0)
            {
                yield return new EmptyQueryResult("Translate text", "Input something to get translation", 1, null);
                yield break;
            }

            string text = CommandLineUtils.Concat(arguments);

            yield return TranslatorAPI switch
            {
                TranslatorAPI.Youdao => new Youdao.YoudaoTranslationQueryResult(this, text),

                _ => throw new Exception("This would never happen")
            };
        }
    }
}
