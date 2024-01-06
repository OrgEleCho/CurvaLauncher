using System.Security.Cryptography;
using System.Text;
using System.Windows.Media;
using CurvaLauncher.Data;
using CurvaLauncher.Plugin.Hashing.Properties;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Plugin.Hashing;

public class HashingPlugin : ISyncPlugin
{
    private readonly Lazy<ImageSource> _laziedImageSource = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);

    public string Name => "Hashing";
    public string Description => "Get a summary of data";
    public System.Windows.Media.ImageSource Icon => _laziedImageSource.Value;

    [PluginOption]
    public string Prefix { get; set; } = "#";

    Dictionary<string, HashAlgorithm> _hashAlgorithmMap = new()
    {
        ["MD5"] = MD5.Create(),
        ["SHA1"] = SHA1.Create(),
        ["SHA256"] = SHA256.Create(),
        ["SHA384"] = SHA384.Create(),
        ["SHA512"] = SHA512.Create(),
    };

    public void Init()
    {

    }

    public IEnumerable<CurvaLauncher.Data.QueryResult> Query(CurvaLauncher.CurvaLauncherContext context, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            yield break;

        CommandLineUtils.Split(query, out var segments);

        var nameSegment = segments[0];

        if (nameSegment.IsQuoted)
            yield break;

        var arguments = segments
            .Skip(1)
            .Select(seg => seg.Value)
            .ToList();

        foreach (var hashAlgorithmKV in _hashAlgorithmMap)
        {
            if ($"{Prefix}{hashAlgorithmKV.Key}".Equals(nameSegment.Value, StringComparison.OrdinalIgnoreCase))
            {
                if (arguments.Count == 0)
                {
                    yield return new EmptyQueryResult("Get summary", $"Input text or file path to get it's summary with {hashAlgorithmKV.Key}", 1, null);
                    yield break;
                }

                bool anyFileExist = arguments.Any(text => File.Exists(text));
                string title = $"Get summary with {hashAlgorithmKV.Key}";

                if (anyFileExist)
                {
                    foreach (var arg in arguments)
                    {
                        if (File.Exists(arg))
                        {
                            yield return new HashingQueryResult(() => File.OpenRead(arg),
                                                                hashAlgorithmKV.Value,
                                                                "Hash file",
                                                                $"Use {hashAlgorithmKV.Key} to get summary of file content",
                                                                1);
                        }

                        yield return new HashingQueryResult(() => new MemoryStream(Encoding.UTF8.GetBytes(arg)),
                                                            hashAlgorithmKV.Value,
                                                            "Hash text",
                                                            $"Use {hashAlgorithmKV.Key} to get summary of input text",
                                                            0.99f);
                    }
                }
                else
                {
                    foreach (var arg in arguments)
                    {
                        yield return new HashingQueryResult(() => new MemoryStream(Encoding.UTF8.GetBytes(arg)),
                                                            hashAlgorithmKV.Value,
                                                            "Hash text",
                                                            $"Use {hashAlgorithmKV.Key} to get summary of input text",
                                                            1);
                    }
                }

                yield break;
            }
        }
    }
}
