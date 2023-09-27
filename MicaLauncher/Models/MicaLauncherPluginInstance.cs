using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicaLauncher.Data;
using MicaLauncher.Plugin;
using MicaLauncher.Utilities;

namespace MicaLauncher.Models
{
    public class MicaLauncherPluginInstance
    {
        public IPlugin Plugin { get; }
        public Task InitTask { get; }

        private MicaLauncherPluginInstance(IPlugin plugin)
        {
            Plugin = plugin;

            if (plugin is IAsyncPlugin asyncPlugin)
                InitTask = asyncPlugin.InitAsync();
            else if (plugin is ISyncPlugin syncPlugin)
                InitTask = Task.Run(syncPlugin.Init);
            else
                throw new ArgumentException("Invalid plugin type", nameof(plugin));
        }

        public IAsyncEnumerable<QueryResult> QueryAsync(MicaLauncherContext context, string query)
        {
            if (Plugin is IAsyncPlugin asyncPlugin)
            {
                return asyncPlugin.QueryAsync(context, query);
            }
            else if (Plugin is ISyncPlugin syncPlugin)
            {
                return AsyncUtils.EnumerableToAsync(syncPlugin.Query(context, query));
            }

            throw new InvalidOperationException();
        }

        public static bool TryCreate(Type type, [NotNullWhen(true)] out MicaLauncherPluginInstance? micaLauncherPlugin)
        {
            micaLauncherPlugin = null;

            if (!type.IsAssignableTo(typeof(IPlugin)))
                return false;

            try
            {
                var plugin = Activator.CreateInstance(type);

                if (plugin is IAsyncPlugin asyncPlugin)
                    micaLauncherPlugin = new MicaLauncherPluginInstance(asyncPlugin);
                else if (plugin is ISyncPlugin syncPlugin)
                    micaLauncherPlugin = new MicaLauncherPluginInstance(syncPlugin);

                return micaLauncherPlugin != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
