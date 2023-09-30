using System.Windows.Media;

namespace CurvaLauncher.Plugin
{
    public interface IPlugin
    {
        public string Name { get; }
        public string Description { get; }

        public ImageSource Icon { get; }
    }
}