using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Apis;

public class ClipboardApi : IClipboardApi
{
    private ClipboardApi() { }
    public static ClipboardApi Instance { get; } = new();

    public void SetImage(ImageSource image) => ClipboardUtils.SetBitmap(image);
    public void SetImage(Image image) => ClipboardUtils.SetBitmap(image);
    public void SetText(string text) => ClipboardUtils.SetText(text);


    public bool HasImage() => System.Windows.Clipboard.ContainsImage();
    public bool HasText() => System.Windows.Clipboard.ContainsText();


    public BitmapSource? GetImage() => System.Windows.Clipboard.GetImage();
    public string GetText() => System.Windows.Clipboard.GetText();
}
 