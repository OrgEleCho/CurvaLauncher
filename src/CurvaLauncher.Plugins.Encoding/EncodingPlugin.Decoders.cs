using System.IO;
using System.Net;
using System.Runtime.CompilerServices;

namespace CurvaLauncher.Plugins.Encoding;

public partial class EncodingPlugin
{
    public class Decoders
    {
        private readonly EncodingPlugin _plugin;

        public Decoders(EncodingPlugin plugin)
        {
            _plugin = plugin;
        }

        public async Task Base64(Stream sourceStream, Stream destStream, CancellationToken cancellationToken)
        {
            int bufferSize = Math.Min((int)sourceStream.Length, _plugin.BufferSize);
            byte[] buffer = new byte[bufferSize];
            byte[] outputBuffer = new byte[System.Buffers.Text.Base64.GetMaxDecodedFromUtf8Length(bufferSize)];

            long totalRead = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                int readCount = await sourceStream.ReadAsync(buffer, cancellationToken);
                totalRead += readCount;

                bool finalBlock = readCount < bufferSize || totalRead == sourceStream.Length;

                var inputSpan = new Memory<byte>(buffer, 0, readCount);
                var outputSpan = new Memory<byte>(outputBuffer, 0, outputBuffer.Length);
                var status = System.Buffers.Text.Base64.DecodeFromUtf8(inputSpan.Span, outputSpan.Span, out int bytesCosumed, out int bytesWritten, finalBlock);

                destStream.Write(outputBuffer, 0, bytesWritten);

                if (finalBlock)
                    break;
            }
        }

        private readonly char[] hexChars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public async Task Hex(Stream sourceStream, Stream destStream, CancellationToken cancellationToken)
        {
            int bufferSize = Math.Min((int)sourceStream.Length, _plugin.BufferSize);
            if (bufferSize % 2 == 1)
                bufferSize += 1;

            byte[] buffer = new byte[bufferSize];
            byte[] outputBuffer = new byte[bufferSize / 2];

            long totalRead = 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                int readCount = await sourceStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                totalRead += readCount;

                bool finalBlock = readCount < bufferSize || totalRead == sourceStream.Length;

                for (int i = 0, j = 0; i < readCount; i += 2, j++)
                {
                    int v1 = GetHexValue(buffer[i]);
                    int v2 = GetHexValue(buffer[i + 1]);

                    outputBuffer[j] = (byte)(v1 * 16 + v2);
                }

                await destStream.WriteAsync(outputBuffer, 0, readCount / 2, cancellationToken);

                if (finalBlock)
                    break;
            }

            static int GetHexValue(byte b)
            {
                if (b >= '0' && b <= '9')
                    return b - '0';
                else if (b >= 'A' && b <= 'F')
                    return b - 'A' + 10;
                else if (b >= 'a' && b <= 'f')
                    return b - 'a' + 10;
                else
                    return 0;
            }
        }

        public async Task Html(Stream sourceStream, Stream destStream, CancellationToken cancellationToken)
        {
            var originLength = _plugin.BufferSize;
            string buffer = new('\0', originLength);
            static void SetLength(string str, int len) => Unsafe.Add(ref Unsafe.As<char, int>(ref Unsafe.AsRef(in str.GetPinnableReference())), -1) = len;

            StreamReader sr = new(sourceStream);
            StreamWriter sw = new(destStream);

            try
            {
                while (true)
                {
                    SetLength(buffer, originLength);
                    var inMemory = buffer.AsMemory();
                    int readlen = await sr.ReadBlockAsync(Unsafe.As<ReadOnlyMemory<char>, Memory<char>>(ref inMemory), cancellationToken); // ReadOnlyMemory和Memory的内部结构相同
                    if (readlen == 0)
                        break;

                    SetLength(buffer, readlen);
                    WebUtility.HtmlDecode(buffer, sw);
                }
            }
            finally
            {
                sw.Flush();
                SetLength(buffer, originLength);
            }
        }
    }
}
