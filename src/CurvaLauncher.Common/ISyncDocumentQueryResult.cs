using System.Windows.Documents;

namespace CurvaLauncher;

public interface ISyncDocumentQueryResult : IQueryResult
{
    public FlowDocument GenerateDocument();
}