namespace CurvaLauncher;

public interface ISyncMenuQueryResult : IQueryResult
{
    public IEnumerable<IQueryResult> GetMenuItems();
}