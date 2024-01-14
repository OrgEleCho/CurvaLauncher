using System.Windows.Media;

namespace CurvaLauncher.Apis;

public interface IImageApi
{
    public ImageSource EmptyImage { get; }
    public ImageSource? GetEmbededIconImage(string filename, int iconSize, int? iconIndex);
    public ImageSource? GetAssociatedIconImage(string filename, int iconSize);
    public ImageSource? GetFileIcon(string filename, int iconSize);
    public ImageSource? CreateFromSvg(string svg);
}
