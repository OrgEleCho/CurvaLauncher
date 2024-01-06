using System.Security.Cryptography;
using System.Windows;
using System.Windows.Media;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.Hashing;

public class HashingQueryResult : AsyncQueryResult
{
    private readonly Func<Stream> _streamFactory;
    private readonly HashAlgorithm _hashAlgorithm;
    private readonly float weight;
    private readonly string title;
    private readonly string description;

    public override float Weight => 1;
    public override string Title => title;
    public override string Description => description;
    public override ImageSource? Icon => null;

    public HashingQueryResult(Func<Stream> streamFactory, HashAlgorithm hashAlgorithm, string title, string description, float weight)
    {
        this.weight = weight;
        this.title = title;
        this.description = description;

        _streamFactory = streamFactory;
        _hashAlgorithm = hashAlgorithm;
    }

    public override async Task InvokeAsync(CancellationToken cancellationToken)
    {
        using var stream = _streamFactory.Invoke();
        var hash = await _hashAlgorithm.ComputeHashAsync(stream, cancellationToken);

        Clipboard.SetText(Convert.ToHexString(hash));
    }
}