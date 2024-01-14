namespace CurvaLauncher.Plugins;

public interface IAsyncPlugin : IPlugin
{
    public Task InitializeAsync() => Task.CompletedTask;
    public Task FinishAsync() => Task.CompletedTask;

    public IAsyncEnumerable<IQueryResult> QueryAsync(string query);
}