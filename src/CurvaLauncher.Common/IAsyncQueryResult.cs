namespace CurvaLauncher;

public interface IAsyncQueryResult : IQueryResult
{
    public Task InvokeAsync(CancellationToken cancellationToken);
}
