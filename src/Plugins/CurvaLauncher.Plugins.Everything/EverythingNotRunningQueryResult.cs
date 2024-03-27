using System.Windows.Media;

namespace CurvaLauncher.Plugins.Everything
{
    public class EverythingNotRunningQueryResult : IQueryResult
    {
        public string Title => "Everything";
        public string Description => "Everything is not running now!";

        public float Weight => 1;

        public ImageSource? Icon => null;
    }
}
