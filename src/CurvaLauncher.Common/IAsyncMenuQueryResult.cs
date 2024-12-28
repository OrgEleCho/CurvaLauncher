namespace CurvaLauncher;

public interface IAsyncMenuQueryResult : IQueryResult
{
    public Task<IEnumerable<IQueryResult>> GetMenuItemsAsync(CancellationToken cancellationToken);
}