using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZXing;

namespace CurvaLauncher.Plugins.ZXing
{
    public class ZXingRecognizeQueryResult : IAsyncQueryResult
    {
        private readonly ZXingPlugin _plugin;

        public string Title => $"Recognize {CodeName}";

        public string Description => $"Use ZXing to recognize {CodeName} from copied image";

        public float Weight => 1;

        public ImageSource? Icon => null;

        public string CodeName { get; }
        public BarcodeFormat? BarcodeFormat { get; }

        public Task InvokeAsync(CancellationToken cancellationToken)
        {
            var imageSource = _plugin.HostContext.ClipboardApi.GetImage();
            if (imageSource == null)
                return Task.CompletedTask;

            MemoryStream ms = new();
            BmpBitmapEncoder encoder = new();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            encoder.Save(ms);

            bool altPressed = _plugin.HostContext.IsAltKeyPressed();

            return Task.Run(() =>
            {
                var reader = new global::ZXing.Windows.Compatibility.BarcodeReader()
                {
                    Options =
                    {
                        CharacterSet = "UTF-8"
                    }
                };

                if (BarcodeFormat is global::ZXing.BarcodeFormat barcodeFormat)
                    reader.Options.PossibleFormats = [barcodeFormat];

                using Bitmap bmp = new Bitmap(ms);

                var result = reader.Decode(bmp);

                if (result == null)
                    return;

                _plugin.HostContext.Dispatcher.Invoke(() =>
                {
                    if (!altPressed)
                        _plugin.HostContext.ClipboardApi.SetText(result.Text);
                    else
                        _plugin.HostContext.Api.ShowText(result.Text, Apis.TextOptions.Default);

                    if (_plugin.AutoOpenDetectedLink && Uri.TryCreate(result.Text, UriKind.Absolute, out var uri))
                        _plugin.HostContext.Api.Open(uri.ToString());
                });
            });
        }

        public ZXingRecognizeQueryResult(ZXingPlugin plugin, string codeName, BarcodeFormat? barcodeFormat)
        {
            _plugin = plugin;
            CodeName = codeName;
            BarcodeFormat = barcodeFormat;
        }
    }
}
