using System.Windows.Media;

namespace CurvaLauncher.Apis;

public interface IClipboardApi
{
    public void SetText(string text);
    public void SetImage(ImageSource image);
    public void SetImage(System.Drawing.Image image);
}