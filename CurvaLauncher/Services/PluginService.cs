using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using CurvaLauncher.Plugin;

namespace CurvaLauncher.Services;

public partial class PluginService
{
    private readonly PathService _pathService;
    private readonly ConfigService _configService;

    public string Path { get; set; } = "Plugins";
    public string ConfigFileName { get; set; } = "Config.json";

    public ObservableCollection<CurvaLauncherPluginInstance> PluginInstances { get; private set; }

    public PluginService(
        PathService pathService,
        ConfigService configService)
    {
        _pathService = pathService;
        _configService = configService;

        LoadPlugins(out var plugins);

        PluginInstances = new(plugins);
    }


    public DirectoryInfo EnsurePluginDirectories()
    {
        DirectoryInfo dir = new(_pathService.GetPath(Path));

        if (!dir.Exists)
            dir.Create();

        return dir;
    }

    private void LoadPlugins(out List<CurvaLauncherPluginInstance> plugins)
    {
        plugins = new List<CurvaLauncherPluginInstance>();

        var dir = EnsurePluginDirectories();
        var dllFiles = dir.GetFiles("*.dll");
        var configFilePath = System.IO.Path.Combine(dir.FullName, ConfigFileName);

        AppConfig config = _configService.Config;

        foreach (FileInfo dllFile in dllFiles)
            if (LoadPlugin(config, dllFile.FullName, out CurvaLauncherPluginInstance? plugin))
            {
                if (config.PluginsConfig != null && config.PluginsConfig.TryGetPropertyValue(plugin.Plugin.GetType().FullName!, out var json))
                {
                    var props = plugin.Plugin.GetType().GetProperties()
                        .Where(p => p.GetCustomAttribute<PluginOptionAttribute>() is not null);

                    foreach (var property in props)
                    {
                        if (json!.AsObject().TryGetPropertyValue(property.Name, out var value) is true)
                        {
                            var type = property.PropertyType;
                            var val = JsonSerializer.Deserialize(value, type);
                            property.SetValue(plugin.Plugin, val);
                        }
                    }
                }
                plugins.Add(plugin);
            }
    }

    private bool LoadPlugin(AppConfig config, string dllFilePath, [NotNullWhen(true)] out CurvaLauncherPluginInstance? plugin)
    {
        plugin = null;

        try
        {
            var assembly = Assembly.LoadFile(dllFilePath);

            Type? pluginType = assembly.ExportedTypes
                .Where(type => type.IsAssignableTo(typeof(ISyncPlugin)) || type.IsAssignableTo(typeof(IAsyncPlugin)))
                .FirstOrDefault();

            if (pluginType == null)
                return false;

            if (!CurvaLauncherPluginInstance.TryCreate(pluginType, out plugin))
                return false;

            var typeName = pluginType.FullName!;
            plugin.IsEnabled = config.DisabledPlugins == null || !config.DisabledPlugins.Contains(typeName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    [RelayCommand]
    public async Task ReloadAllPlugins()
    {
        foreach (var plugin in PluginInstances.Where(ins => ins.IsEnabled))
        {
            if (plugin.Plugin is ISyncPlugin syncPlugin)
                syncPlugin.Initialize();
            else if (plugin.Plugin is IAsyncPlugin asyncPlugin)
                await asyncPlugin.InitializeAsync();
        }
    }
}