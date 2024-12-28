using System.Windows.Documents;

namespace CurvaLauncher;

public interface IAsyncDocumentQueryResult : IQueryResult
{
    public Task<FlowDocument> GenerateDocumentAsync(CancellationToken cancellationToken);
}