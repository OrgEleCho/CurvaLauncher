using System.Windows.Media;
using CurvaLauncher.Data;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Plugin.QuickWebSearch
{
    public class QuickWebSearchQueryResult : SyncQueryResult
    {
        private readonly ImageSource? _icon;

        public override string Title => $"Web search for {Keyword}";

        public override string Description => $"Use {Engine} to search the web for '{Keyword}'";

        public override float Weight => 1;

        public override ImageSource? Icon => _icon;

        public string Engine { get; }
        public string Keyword { get; }
        public string Url { get; }

        public override void Invoke()
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
