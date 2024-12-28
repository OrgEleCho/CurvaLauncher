using System.Windows.Documents;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Test
{
    public class TestMenuQueryResult : ISyncMenuQueryResult
    {
        public string Title => "Test Menu";

        public string Description => "Show test menu";

        public float Weight => 1;

        public ImageSource? Icon => null;

        public IEnumerable<IQueryResult> GetMenuItems()
        {
            yield return new TestQueryResult("Show test message box", string.Empty, 1);
            yield return new TestDocumentQueryResult();
            yield return new TestQueryResultWithPreview("Show test message box", string.Empty, 1, new System.Windows.Documents.FlowDocument()
            {
                Blocks =
                {
                    new Paragraph(new Run("QWQ"))
                    {
                        FontSize = 22,
                    }
                }
            });
        }
    }
}