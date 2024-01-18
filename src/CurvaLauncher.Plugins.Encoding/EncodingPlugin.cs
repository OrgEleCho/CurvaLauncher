using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Media;
using CurvaLauncher.Apis;
using CurvaLauncher.Plugins.Encoding.Properties;

namespace CurvaLauncher.Plugins.Encoding;

/// <summary>
/// 各种编码, Base64 编码, URI 编码, 十六进制, 正则字符编解码
/// </summary>
public partial class EncodingPlugin : CommandSyncI18nPlugin
{
    const string CmdBase64Enc = "base64enc";
    const string CmdBase64Dec = "base64dec";
    const string CmdHtmlEnc = "htmlenc";
    const string CmdHtmlDec = "htmldec";
    const string CmdUriEnc = "urienc";
    const string CmdUriDec = "uridec";
    const string CmdHexEnc = "hexenc";
    const string CmdHexDec = "hexdec";
    const string CmdRegEnc = "regenc";
    const string CmdRegDec = "regdec";


    [PluginI18nOption("StrBufferSize")]
    public int BufferSize { get; set; } = 1024;


    readonly Encoders _encoders;
    readonly Decoders _decoders;
    readonly Dictionary<string, AsyncCodec> _codecs;
    public UTF8Encoding Encoding { get; } = new UTF8Encoding(false, false);

    public EncodingPlugin(CurvaLauncherContext context) : base(context)
    {
        Icon = context.ImageApi.CreateFromSvg(Resources.IconSvg)!;

        _encoders = new Encoders(this);
        _decoders = new Decoders(this);

        _codecs = new()
        {
            [CmdBase64Enc] = _encoders.Base64,
            [CmdBase64Dec] = _decoders.Base64,
            [CmdHtmlEnc] = _encoders.Html!,
            [CmdHtmlDec] = null!,
            [CmdUriEnc] = null!,
            [CmdUriDec] = null!,
            [CmdHexEnc] = _encoders.Hex,
            [CmdHexDec] = _decoders.Hex,
            [CmdRegEnc] = null!,
            [CmdRegDec] = null!,
        };
    }

    public override object NameKey => "StrPluginName";
    public override object DescriptionKey => "StrPluginDescription";

    public override IEnumerable<string> CommandNames
    {
        get
        {
            yield return CmdBase64Enc;
            yield return CmdBase64Dec;
            yield return CmdUriEnc;
            yield return CmdUriDec;
            yield return CmdHexEnc;
            yield return CmdHexDec;
            yield return CmdRegEnc;
            yield return CmdRegDec;
        }
    }

    public override ImageSource Icon { get; }

    public override IEnumerable<IQueryResult> ExecuteCommand(string commandName, CommandLineSegment[] arguments)
    {
        string codecName = commandName.ToLower();
        if (_codecs.TryGetValue(codecName, out var asyncCodec))
        {
            if (arguments.Length == 0)
            {
                yield return new EmptyQueryResult($"Codec {codecName}", $"Input text or file path to get the result of {codecName}", 1, null);
                yield break;
            }

            bool anyFileExist = arguments.Any(text => File.Exists(text.Value));
            string title = $"Codec {codecName}";

            var textFactories = arguments
                .Select(arg => (Func<Stream>)(() => new MemoryStream(Encoding.GetBytes(arg.Value))));

            if (anyFileExist)
            {
                var fileFactories = arguments
                    .Where(arg => File.Exists(arg.Value))
                    .Select(arg => (Func<Stream>)(() => File.OpenRead(arg.Value)));

                var fileOutputFactories = arguments
                    .Where(arg => File.Exists(arg.Value))
                    .Select(origin => Path.ChangeExtension(origin.Value, $"{Path.GetExtension(origin.Value)}.{codecName}"))
                    .Select(filename => (Func<Stream>)(() => File.Create(filename)));

                yield return new EncodingQueryResult(this, fileFactories, fileOutputFactories, asyncCodec,
                    $"Codec {codecName}",
                    $"Use {codecName} to encode or decode file content",
                    1);

                yield return new EncodingQueryResult(this, textFactories, null, asyncCodec,
                    $"Codec {codecName}",
                    $"Use {codecName} to encode or decode file content",
                    .9f);
            }
            else
            {
                yield return new EncodingQueryResult(this, textFactories, null, asyncCodec,
                    $"Codec {codecName}",
                    $"Use {codecName} to encode or decode file content",
                    1);
            }
        }
    }

    public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
    {
        foreach (var item in base.GetI18nResourceDictionaries())
            yield return item;

        yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
    }

    public delegate Task AsyncCodec(Stream sourceStream, Stream destStream, CancellationToken cancellationToken);
}
