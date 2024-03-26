using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CurvaLauncher.Apis;
using ZXing;
using ZXing.Windows.Compatibility;

namespace CurvaLauncher.Plugins.ZXing
{
    public class ZXingGenerateQueryResult : IAsyncQueryResult
    {
        public string Title => $"Generate {CodeName}";

        public string Description => $"Use ZXing to generate {CodeName} based on given content";

        public float Weight => 1;

        public ImageSource? Icon => null;

        public BarcodeFormat BarcodeFormat { get; }
        public int Width { get; }
        public int Height { get; }
        public int Margin { get; }
        public string Content { get; }
        public CurvaLauncherContext HostContext { get; }
        public string CodeName { get; }

        public ZXingGenerateQueryResult(CurvaLauncherContext hostContext, string codeName, BarcodeFormat barcodeFormat, int width, int height, int margin, string content)
        {
            HostContext = hostContext;
            CodeName = codeName;
            BarcodeFormat = barcodeFormat;
            Width = width;
            Height = height;
            Margin = margin;
            Content = content;
        }

        public Task InvokeAsync(CancellationToken cancellationToken)
        {
            bool altPressed = HostContext.IsAltKeyPressed();

            return Task.Run(() =>
            {
                var writer = new BarcodeWriter()
                {
                    Format = BarcodeFormat,
                    Options = new()
                    {
                        Width = Width,
                        Height = Height,
                        Margin = Margin,
                        NoPadding = true,
                        Hints = { { EncodeHintType.CHARACTER_SET, "UTF-8" } }
                    },
                };

                using var image = writer.Write(Content);

                if (!altPressed)
                {
                    HostContext.ClipboardApi.SetImage(image);
                }
                else
                {
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                    HostContext.Dispatcher.Invoke(() =>
                    {
                        BitmapImage bmp = new BitmapImage();

                        bmp.BeginInit();
                        bmp.StreamSource = ms;
                        bmp.EndInit();

                        HostContext.Api.ShowImage(bmp, ImageOptions.Default);
                    });
                }
            });
        }
    }
}
