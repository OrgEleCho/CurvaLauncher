using System.Windows.Threading;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin
{
    public interface ISyncPlugin : IPlugin
    {
        public void Init();
        public IEnumerable<QueryResult> Query(CurvaLauncherContext context, string query);
    }
}