namespace CurvaLauncher;

public interface IAsyncActionQueryResult : IQueryResult
{
    public Task InvokeAsync(CancellationToken cancellationToken);
}
