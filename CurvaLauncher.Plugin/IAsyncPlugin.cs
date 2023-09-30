using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin
{
    public interface IAsyncPlugin : IPlugin
    {
        public Task InitAsync();
        public IAsyncEnumerable<QueryResult> QueryAsync(CurvaLauncherContext context, string query);
    }
}