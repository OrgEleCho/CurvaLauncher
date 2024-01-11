namespace CurvaLauncher.Plugin;

public abstract class CurvaLauncherSyncPlugin : CurvaLauncherPlugin, ICurvaLauncherSyncPlugin
{
    protected CurvaLauncherSyncPlugin(CurvaLauncherContext context) : base(context)
    {
    }

    public virtual void Initialize() { }
    public virtual void Finish() { }

    public abstract IEnumerable<IQueryResult> Query(string query);
}
