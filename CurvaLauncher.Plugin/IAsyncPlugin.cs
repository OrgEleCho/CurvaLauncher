namespace CurvaLauncher.Plugin;

public interface IAsyncPlugin : IPlugin
{
    Task InitializeAsync();
    Task FinishAsync();
    IAsyncEnumerable<IQueryResult> QueryAsync(CurvaLauncherContext context, string query);
}