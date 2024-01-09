using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugin.Hashing;

public class HashingQueryResult : IAsyncQueryResult
{
    private readonly IEnumerable<Func<Stream>> _streamFactories;
    private readonly HashAlgorithm _hashAlgorithm;
    private readonly float weight;
    private readonly string title;
    private readonly string description;

    public float Weight => 1;
    public string Title => title;
    public string Description => description;
    public ImageSource? Icon => null;

    public HashingQueryResult(IEnumerable<Func<Stream>> streamFactories, HashAlgorithm hashAlgorithm, string title, string description, float weight)
    {
        this.weight = weight;
        this.title = title;
        this.description = description;

        _streamFactories = streamFactories;
        _hashAlgorithm = hashAlgorithm;
    }

    public async Task InvokeAsync(CancellationToken cancellationToken)
    {
        List<string> results = new();

        foreach (var streamFactory in _streamFactories)
        {
            using var stream = streamFactory.Invoke();
            var hash = await _hashAlgorithm.ComputeHashAsync(stream, cancellationToken);

            results.Add(Convert.ToHexString(hash));
        }

        Clipboard.SetText(string.Join(Environment.NewLine, results));
    }
}