using System.Globalization;
using System.Windows.Media;
using CurvaLauncher.Apis;
using CurvaLauncher.Plugins.ZXing.Properties;
using ZXing;

namespace CurvaLauncher.Plugins.ZXing
{
    public class ZXingPlugin : CommandSyncI18nPlugin
    {
        public override object NameKey => "StrPluginName";
        public override object DescriptionKey => "StrPluginDescription";

        [PluginI18nOption("StrZXingCommandName")]
        public string ZXingCommandName { get; set; } = "ZXing";

        [PluginI18nOption("StrQrCodeCommandName")]
        public string QrCodeCommandName { get; set; } = "QrCode";

        [PluginI18nOption("StrOutputImageWidth")]
        public int OutputImageWidth { get; set; } = 200;

        [PluginI18nOption("StrOutputImageHeight")]
        public int OutputImageHeight { get; set; } = 200;

        [PluginI18nOption("StrAutoOpenDetectedLink")]
        public bool AutoOpenDetectedLink { get; set; } = false;

        [PluginI18nOption("StrPlaceholderForEmptyResult")]
        public string PlaceholderForEmptyResult { get; set; } = "[None]";

        public override ImageSource Icon { get; }

        public override IEnumerable<string> CommandNames
        {
            get
            {
                yield return ZXingCommandName;
                yield return QrCodeCommandName;
            }
        }

        public ZXingPlugin(CurvaLauncherContext context) : base(context)
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

        public override IEnumerable<IQueryResult> ExecuteCommand(string commandName, CommandLineSegment[] arguments)
        {
            string content = HostContext.CommandLineApi.Concat(arguments);

            (string codeName, BarcodeFormat barcodeFormat)? codeValues = commandName.ToLower() switch
            {
                string name when name.Equals(QrCodeCommandName, StringComparison.OrdinalIgnoreCase) => ("QR Code", BarcodeFormat.QR_CODE),
                _ => null
            };

            if (codeValues == null)
            {
                if (commandName.Equals(ZXingCommandName, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.IsNullOrWhiteSpace(content))
                        yield return new ZXingRecognizeQueryResult(this, "Any code", null);
                    else
                        yield return new EmptyQueryResult($"ZXing", $"Copy some image for code recognition", 1, null);
                }

                yield break;
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                if (!HostContext.ClipboardApi.HasImage())
                    yield return new EmptyQueryResult($"Generate {codeValues.Value.codeName}", $"Enter the content to use to generate a {codeValues.Value.codeName}", 1, null);
                else
                    yield return new ZXingRecognizeQueryResult(this, codeValues.Value.codeName, BarcodeFormat.QR_CODE);
            }
            else
            {
                yield return new ZXingGenerateQueryResult(HostContext, codeValues.Value.codeName, codeValues.Value.barcodeFormat, OutputImageWidth, OutputImageHeight, 5, content);
            }
        }
    }
}
