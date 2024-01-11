namespace CurvaLauncher.Plugin;

public interface ICurvaLauncherSyncPlugin : ICurvaLauncherPlugin
{
    public void Initialize() { }
    public void Finish() { }

    public IEnumerable<IQueryResult> Query(string query);
}