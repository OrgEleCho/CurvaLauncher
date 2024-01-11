using System.Windows.Media;
using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugin.QuickWebSearch
{
    public class QuickWebSearchQueryResult : ISyncQueryResult
    {
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
            ShellUtils.Start(Url);
        }

        public QuickWebSearchQueryResult(string engine, string kwd, string url, ImageSource? icon)
        {
            Engine = engine;
            Keyword = kwd;
            Url = url;
            _icon = icon;
        }
    }
}
