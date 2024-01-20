using System.Drawing;
using System.Windows.Media;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Apis
{
    public class ClipboardApi : IClipboardApi
    {
        private ClipboardApi() { }
        public static ClipboardApi Instance { get; } = new();

        public void SetImage(ImageSource image) => ClipboardUtils.SetBitmap(image);
        public void SetImage(Image image) => ClipboardUtils.SetBitmap(image);
        public void SetText(string text) => ClipboardUtils.SetText(text);
    }
}
