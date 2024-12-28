using System.Windows.Documents;

namespace CurvaLauncher.Models.ImmediateResults
{
    public class DocumentResult : ImmediateResult
    {
        public FlowDocument Document { get; }

        public DocumentResult(FlowDocument document)
        {
            Document = document;
        }
    }
}
