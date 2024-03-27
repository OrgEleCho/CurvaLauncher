using System.Windows.Media;

namespace CurvaLauncher.Apis;

public interface ICommonApi
{
    public void Open(string name);
    public void OpenExecutable(string file);

    public void ShowInFileExplorer(string path);
    public void ShowPropertiesWindow(string path);

    public void ShowText(string text, TextOptions options);
    public void ShowImage(ImageSource image, ImageOptions options);
}
