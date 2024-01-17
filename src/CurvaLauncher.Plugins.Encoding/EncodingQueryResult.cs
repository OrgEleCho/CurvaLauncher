using System.IO;
using System.Text;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Encoding
{
    public class EncodingQueryResult : IAsyncQueryResult
    {
        private readonly EncodingPlugin _plugin;
        private readonly IEnumerable<Func<Stream>> _sourceStreamFactories;
        private readonly IEnumerable<Func<Stream>>? _destStreamFactories;
        private readonly EncodingPlugin.AsyncCodec _asyncEncoder;

        public string Title { get; }

        public string Description { get; }

        public float Weight { get; }

        public ImageSource? Icon => null;

        public EncodingQueryResult(EncodingPlugin plugin, IEnumerable<Func<Stream>> sourceStreamFactories, IEnumerable<Func<Stream>>? destStreamFactories, EncodingPlugin.AsyncCodec asyncEncoder, string title, string description, float weight)
        {
            Title = title;
            Description = description;
            Weight = weight;

            _plugin = plugin;
            _sourceStreamFactories = sourceStreamFactories;
            _destStreamFactories = destStreamFactories;
            _asyncEncoder = asyncEncoder;
        }

        public async Task InvokeAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_destStreamFactories != null)
                {
                    var sourceStreamEnumerator = _sourceStreamFactories.GetEnumerator();
                    var destStreamEnumerator = _destStreamFactories.GetEnumerator();

                    while (sourceStreamEnumerator.MoveNext() && destStreamEnumerator.MoveNext())
                    {
                        using var sourceStream = sourceStreamEnumerator.Current.Invoke();
                        using var destStream = destStreamEnumerator.Current.Invoke();

                        await _asyncEncoder.Invoke(sourceStream, destStream, cancellationToken);
                    }
                }
                else
                {
                    List<string> results = new();
                    MemoryStream ms = new();

                    foreach (var sourceStreamFactory in _sourceStreamFactories)
                    {
                        using var sourceStream = sourceStreamFactory.Invoke();

                        ms.SetLength(0);
                        await _asyncEncoder.Invoke(sourceStream, ms, cancellationToken);

                        byte[] msBuffer = ms.GetBuffer();
                        string str = _plugin.Encoding.GetString(msBuffer, 0, (int)ms.Length);
                        results.Add(str);
                    }

                    _plugin.HostContext.Api.SetClipboardText(string.Join(Environment.NewLine, results));
                }
            }
            catch (OperationCanceledException)
            { }
        }
    }
}
