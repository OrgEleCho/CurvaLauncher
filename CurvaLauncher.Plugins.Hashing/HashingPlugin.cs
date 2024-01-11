using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media;
using CurvaLauncher.Apis;
using CurvaLauncher.Plugins.Hashing.Properties;

namespace CurvaLauncher.Plugins.Hashing;

public class HashingPlugin : CommandSyncI18nPlugin
{
    private readonly Lazy<ImageSource> _laziedImageSource;

    public override ImageSource Icon => _laziedImageSource.Value;
    public override IEnumerable<string> CommandNames => _hashAlgorithmMap.Keys;

    public override object NameKey => "StrPluginName";
    public override object DescriptionKey => "StrPluginDescription";


    static Dictionary<string, HashAlgorithm> _hashAlgorithmMap = new()
    {
        ["MD5"] = MD5.Create(),
        ["SHA1"] = SHA1.Create(),
        ["SHA256"] = SHA256.Create(),
        ["SHA384"] = SHA384.Create(),
        ["SHA512"] = SHA512.Create(),
    };

    public HashingPlugin(CurvaLauncherContext context) : base(context)
    {
        Prefix = "#";
        _laziedImageSource = new Lazy<ImageSource>(() => HostContext.ImageApi.CreateFromSvg(Resources.IconSvg)!);
    }

    public override IEnumerable<IQueryResult> ExecuteCommand(CurvaLauncherContext context, string commandName, CommandLineSegment[] arguments)
    {
        string algorithmName = commandName.ToUpper();
        if (_hashAlgorithmMap.TryGetValue(algorithmName, out var hashAlgorithm))
        {
            if (arguments.Length == 0)
            {
                yield return new EmptyQueryResult("Get summary", $"Input text or file path to get it's summary with {algorithmName}", 1, null);
                yield break;
            }

            bool anyFileExist = arguments.Any(text => File.Exists(text.Value));
            string title = $"Get summary with {algorithmName}";

            var textFactories = arguments
                    .Select(arg => (Func<Stream>)(() => new MemoryStream(Encoding.UTF8.GetBytes(arg.Value))));

            if (anyFileExist)
            {
                var fileFactories = arguments
                        .Where(arg => File.Exists(arg.Value))
                        .Select(arg => (Func<Stream>)(() => File.OpenRead(arg.Value)));

                yield return new HashingQueryResult(fileFactories,
                                                    hashAlgorithm,
                                                    "Hash file",
                                                    $"Use {algorithmName} to get summary of file content",
                                                    1);

                yield return new HashingQueryResult(textFactories,
                                                    hashAlgorithm,
                                                    "Hash text",
                                                    $"Use {algorithmName} to get summary of input text",
                                                    0.9f);
            }
            else
            {
                yield return new HashingQueryResult(textFactories,
                                                    hashAlgorithm,
                                                    "Hash text",
                                                    $"Use {algorithmName} to get summary of input text",
                                                    1);
            }
        }
    }

    public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
    {
        yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
    }
}
