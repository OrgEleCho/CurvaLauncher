using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Hashing;

public class HashingQueryResult : IAsyncActionQueryResult
{
    private readonly CurvaLauncherContext _hostContext;
    private readonly IEnumerable<Func<Stream>> _streamFactories;
    private readonly HashAlgorithm _hashAlgorithm;
    private readonly float weight;
    private readonly string title;
    private readonly string description;

    public float Weight => weight;
    public string Title => title;
    public string Description => description;
    public ImageSource? Icon => null;


    public HashingQueryResult(CurvaLauncherContext hostContext, IEnumerable<Func<Stream>> streamFactories, HashAlgorithm hashAlgorithm, string title, string description, float weight)
    {
        this.weight = weight;
        this.title = title;
        this.description = description;

        _hostContext = hostContext;
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

        _hostContext.ClipboardApi.SetText(string.Join(Environment.NewLine, results));
    }
}