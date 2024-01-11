namespace CurvaLauncher.Plugin;

public abstract class CurvaLauncherAsyncPlugin : CurvaLauncherPlugin, ICurvaLauncherAsyncPlugin
{
    protected CurvaLauncherAsyncPlugin(CurvaLauncherContext context) : base(context)
    {
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;
    public virtual Task FinishAsync() => Task.CompletedTask;

    public abstract IAsyncEnumerable<IQueryResult> QueryAsync(string query);
}
