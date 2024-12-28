using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Test
{
    public class TestDocumentQueryResult : ISyncDocumentQueryResult
    {
        public string Title => "Document Hello world";

        public string Description => "QWQ";

        public float Weight => 1;

        public ImageSource? Icon => null;

        public FlowDocument GenerateDocument()
        {
            return new FlowDocument()
            {
                Blocks =
                {
                    new Paragraph(new Run("Hello world"))
                    {
                        FontSize = 24,
                        FontWeight = FontWeights.Bold,
                    }
                }
            };
        }
    }
}