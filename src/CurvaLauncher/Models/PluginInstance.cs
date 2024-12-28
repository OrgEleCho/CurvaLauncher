using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CurvaLauncher.Plugins;
using CurvaLauncher.PluginInteraction;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace CurvaLauncher.Models;

public partial class PluginInstance : ObservableObject
{
    public IPlugin Plugin { get; }
    public Task InitTask { get; private set; } = Task.CompletedTask;


    private PluginInstance(IPlugin plugin)
    {
        Plugin = plugin;
        if (plugin is II18nPlugin i18nPlugin)
        {
            var assembly = plugin.GetType().Assembly;
        }

        if (plugin is not ISyncPlugin && plugin is not IAsyncPlugin)
            throw new ArgumentException("Invalid plugin", nameof(plugin));
    }

    [ObservableProperty]
    private bool _isEnabled = false;

    [ObservableProperty]
    private float _weight = 1;

    partial void OnIsEnabledChanged(bool value)
    {
        if (value)
        {
            if (Plugin is IAsyncPlugin asyncPlugin)
            {
                InitTask = asyncPlugin.InitializeAsync();
            }
            else if (Plugin is ISyncPlugin syncPlugin)
            {
                syncPlugin.Initialize();
                InitTask = Task.CompletedTask;
            }
        }
        else
        {
            if (Plugin is IAsyncPlugin asyncPlugin)
            {
                InitTask = asyncPlugin.FinishAsync();
            }
            else if (Plugin is ISyncPlugin syncPlugin)
            {
                syncPlugin.Finish();
                InitTask = Task.CompletedTask;
            }
        }
    }

    public async IAsyncEnumerable<IQueryResult> QueryAsync(string query)
    {
        if (Plugin is IAsyncPlugin asyncPlugin)
        {
            await foreach (var result in asyncPlugin.QueryAsync(query))
                yield return result;
        }
        else if (Plugin is ISyncPlugin syncPlugin)
        {
            foreach (var result in syncPlugin.Query(query))
                yield return result;

        }
    }

    public static bool TryCreate(Type type, [NotNullWhen(true)] out PluginInstance? curvaLauncherPlugin)
    {
        curvaLauncherPlugin = null;

        if (!type.IsAssignableTo(typeof(IPlugin)))
            return false;

        try
        {
            var plugin = Activator.CreateInstance(type, CurvaLauncherContextImpl.Instance);

            if (plugin is IAsyncPlugin asyncPlugin)
                curvaLauncherPlugin = new PluginInstance(asyncPlugin);
            else if (plugin is ISyncPlugin syncPlugin)
                curvaLauncherPlugin = new PluginInstance(syncPlugin);

            return curvaLauncherPlugin != null;
        }
        catch
        {
            return false;
        }
    }
}
