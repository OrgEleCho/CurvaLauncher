using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CurvaLauncher.Apis;

public interface IClipboardApi
{
    public void SetText(string text);
    public void SetImage(ImageSource image);
    public void SetImage(System.Drawing.Image image);

    public bool HasImage();
    public bool HasText();

    public BitmapSource? GetImage();
    public string GetText();
}