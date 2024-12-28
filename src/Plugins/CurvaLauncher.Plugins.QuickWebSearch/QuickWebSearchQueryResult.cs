using System.Windows.Media;
using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugins.QuickWebSearch
{
    public class QuickWebSearchQueryResult : ISyncActionQueryResult
    {
        private readonly CurvaLauncherContext _context;
        private readonly ImageSource? _icon;

        public string Title => $"Web search for {Keyword}";

        public string Description => $"Use {Engine} to search the web for '{Keyword}'";

        public float Weight => 1;

        public ImageSource? Icon => _icon;

        public string Engine { get; }
        public string Keyword { get; }
        public string Url { get; }

        public void Invoke()
        {
            _context.Api.Open(Url);
        }

        public QuickWebSearchQueryResult(CurvaLauncherContext context, string engine, string kwd, string url, ImageSource? icon)
        {
            _context = context;
            Engine = engine;
            Keyword = kwd;
            Url = url;
            _icon = icon;
        }
    }
}
