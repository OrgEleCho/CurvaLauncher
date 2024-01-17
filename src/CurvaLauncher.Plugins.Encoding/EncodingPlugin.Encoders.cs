using System.IO;

namespace CurvaLauncher.Plugins.Encoding
{
    public partial class EncodingPlugin
    {


        public class Encoders
        {
            private readonly EncodingPlugin _plugin;

            public Encoders(EncodingPlugin plugin)
            {
                _plugin = plugin;
            }

            public async Task Base64(Stream sourceStream, Stream destStream, CancellationToken cancellationToken)
            {
                int bufferSize = Math.Min((int)sourceStream.Length, _plugin.BufferSize);
                byte[] buffer = new byte[bufferSize];
                byte[] outputBuffer = new byte[System.Buffers.Text.Base64.GetMaxEncodedToUtf8Length(bufferSize)];

                long totalRead = 0;

                while (!cancellationToken.IsCancellationRequested)
                {
                    int readCount = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    totalRead += readCount;

                    bool finalBlock = readCount < bufferSize || totalRead == sourceStream.Length;

                    var inputSpan = new Memory<byte>(buffer, 0, readCount);
                    var outputSpan = new Memory<byte>(outputBuffer, 0, outputBuffer.Length);
                    var status = System.Buffers.Text.Base64.EncodeToUtf8(inputSpan.Span, outputSpan.Span, out int bytesCosumed, out int bytesWritten, finalBlock);

                    destStream.Write(outputBuffer, 0, bytesWritten);

                    if (finalBlock)
                        break;
                }
            }

            readonly char[] hexChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            public async Task Hex(Stream sourceStream, Stream destStream, CancellationToken cancellationToken)
            {
                int bufferSize = Math.Min((int)sourceStream.Length, _plugin.BufferSize);
                byte[] buffer = new byte[bufferSize];
                char[] outputBuffer = new char[bufferSize * 2];

                long totalRead = 0;

                using StreamWriter writer = new(destStream, _plugin.Encoding, leaveOpen: true);
                while (!cancellationToken.IsCancellationRequested)
                {
                    int readCount = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                    totalRead += readCount;

                    bool finalBlock = readCount < bufferSize || totalRead == sourceStream.Length;

                    for (int i = 0; i < readCount; i++)
                    {
                        outputBuffer[i * 2] = hexChars[buffer[i] / 16];
                        outputBuffer[i * 2 + 1] = hexChars[buffer[i] % 16];
                    }

                    writer.Write(outputBuffer, 0, readCount * 2);

                    if (finalBlock)
                        break;
                }
            }
        }
    }
}
