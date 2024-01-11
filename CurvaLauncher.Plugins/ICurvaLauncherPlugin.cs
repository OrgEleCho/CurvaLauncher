using System.Windows.Media;

namespace CurvaLauncher.Plugin;

public interface ICurvaLauncherPlugin
{
    public string Name { get; }
    public string Description { get; }
    public ImageSource Icon { get; }
}