using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin;

public interface ISyncPlugin : IPlugin
{
    void Init();
    IEnumerable<QueryResult> Query(CurvaLauncherContext context, string query);
}