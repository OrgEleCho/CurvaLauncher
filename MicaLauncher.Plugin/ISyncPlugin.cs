using System.Windows.Threading;
using MicaLauncher.Data;

namespace MicaLauncher.Plugin
{
    public interface ISyncPlugin : IPlugin
    {
        public void Init();
        public IEnumerable<QueryResult> Query(MicaLauncherContext context, string query);
    }
}