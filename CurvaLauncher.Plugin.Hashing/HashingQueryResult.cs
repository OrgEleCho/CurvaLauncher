using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.Hashing;

public class HashingQueryResult : AsyncQueryResult
{
    private readonly IEnumerable<Func<Stream>> _streamFactories;
    private readonly HashAlgorithm _hashAlgorithm;
    private readonly float weight;
    private readonly string title;
    private readonly string description;

    public override float Weight => 1;
    public override string Title => title;
    public override string Description => description;
    public override ImageSource? Icon => null;

    public HashingQueryResult(IEnumerable<Func<Stream>> streamFactories, HashAlgorithm hashAlgorithm, string title, string description, float weight)
    {
        this.weight = weight;
        this.title = title;
        this.description = description;

        _streamFactories = streamFactories;
        _hashAlgorithm = hashAlgorithm;
    }

    public override async Task InvokeAsync(CancellationToken cancellationToken)
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