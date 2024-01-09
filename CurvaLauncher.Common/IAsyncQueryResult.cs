namespace CurvaLauncher.Data;

public abstract class AsyncQueryResult : QueryResult
{
    public abstract Task InvokeAsync(CancellationToken cancellationToken);
}
