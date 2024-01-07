using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CurvaLauncher.Data;
using CurvaLauncher.Plugin;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Models;

public class CurvaLauncherPluginInstance
{
    public IPlugin Plugin { get; }
    public Task InitTask { get; }

    private CurvaLauncherPluginInstance(IPlugin plugin)
    {
        Plugin = plugin;

        if (plugin is IAsyncPlugin asyncPlugin)
            InitTask = asyncPlugin.InitAsync();
        else if (plugin is ISyncPlugin syncPlugin)
            InitTask = Task.Run(syncPlugin.Init);
        else
            throw new ArgumentException("Invalid plugin type", nameof(plugin));
    }

    public async IAsyncEnumerable<QueryResult> QueryAsync(CurvaLauncherContext context, string query)
    {
        if (Plugin is IAsyncPlugin asyncPlugin)
        {
            await foreach (var result in asyncPlugin.QueryAsync(context, query))
                yield return result;

            //return asyncPlugin.QueryAsync(context, query);
        }
        else if (Plugin is ISyncPlugin syncPlugin)
        {
            //return AsyncUtils.EnumerableToAsync(syncPlugin.Query(context, query));

            foreach (var result in syncPlugin.Query(context, query))
                yield return result;

        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public static bool TryCreate(Type type, [NotNullWhen(true)] out CurvaLauncherPluginInstance? CurvaLauncherPlugin)
    {
        CurvaLauncherPlugin = null;

        if (!type.IsAssignableTo(typeof(IPlugin)))
            return false;

        try
        {
            var plugin = Activator.CreateInstance(type);

            if (plugin is IAsyncPlugin asyncPlugin)
                CurvaLauncherPlugin = new CurvaLauncherPluginInstance(asyncPlugin);
            else if (plugin is ISyncPlugin syncPlugin)
                CurvaLauncherPlugin = new CurvaLauncherPluginInstance(syncPlugin);

            return CurvaLauncherPlugin != null;
        }
        catch
        {
            return false;
        }
    }
}
