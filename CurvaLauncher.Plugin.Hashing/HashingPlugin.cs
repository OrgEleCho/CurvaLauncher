using System.Security.Cryptography;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Plugin.Hashing
{
    public class HashingPlugin : ISyncPlugin
    {
        public string Name => "Hashing";

        public string Description => "Get a summary of data";

        public System.Windows.Media.ImageSource Icon => throw new NotImplementedException();

        Dictionary<string, HashAlgorithm> _hashAlgorithmMap = new()
        {
            ["MD5"] = MD5.Create(),
            ["SHA1"] = SHA1.Create(),
            ["SHA256"] = SHA256.Create(),
            ["SHA512"] = SHA512.Create(),
        };

        public void Init()
        {
            throw new NotImplementedException();
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
                if ($"#{hashAlgorithmKV.Key}".Equals(nameSegment.Value, StringComparison.OrdinalIgnoreCase))
                {
                    bool anyFileExist = arguments.Any(text => File.Exists(text));

                    if (anyFileExist)
                    {
                        yield return new 
                    }
                    else
                    {

                    }

                    yield break;
                }
            }
        }
    }
}
