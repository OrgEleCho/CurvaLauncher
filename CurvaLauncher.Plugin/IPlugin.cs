using System.Windows.Media;

namespace CurvaLauncher.Plugin;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }

    ImageSource Icon { get; }
}
