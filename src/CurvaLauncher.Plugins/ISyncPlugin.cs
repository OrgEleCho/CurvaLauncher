namespace CurvaLauncher.Plugins;

public interface ISyncPlugin : IPlugin
{
    public void Initialize() { }
    public void Finish() { }

    public IEnumerable<IQueryResult> Query(string query);
}