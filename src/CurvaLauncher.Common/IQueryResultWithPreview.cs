using System.Windows.Documents;

namespace CurvaLauncher;

public interface IQueryResultWithPreview
{
    public FlowDocument GeneratePreview();
}
