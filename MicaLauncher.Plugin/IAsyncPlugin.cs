using MicaLauncher.Data;

namespace MicaLauncher.Plugin
{
    public interface IAsyncPlugin : IPlugin
    {
        public Task InitAsync();
        public IAsyncEnumerable<QueryResult> QueryAsync(MicaLauncherContext context, string query);
    }
}