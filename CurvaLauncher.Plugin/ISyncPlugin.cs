namespace CurvaLauncher.Plugin;

public interface ISyncPlugin : IPlugin
{
    void Initialize();
    void Finish();

    IEnumerable<IQueryResult> Query(CurvaLauncherContext context, string query);
}