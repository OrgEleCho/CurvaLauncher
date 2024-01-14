namespace CurvaLauncher.Plugins;

public abstract class SyncPlugin : Plugin, ISyncPlugin
{
    protected SyncPlugin(CurvaLauncherContext context) : base(context)
    {
    }

    public virtual void Initialize() { }
    public virtual void Finish() { }

    public abstract IEnumerable<IQueryResult> Query(string query);
}
