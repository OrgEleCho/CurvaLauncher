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

        JsonObject? config = _configService.Config.PluginsConfig;

//#if DEBUG
//            var debugDir = Directory.GetCurrentDirectory();
//            var projectRoot = System.IO.Path.Combine(debugDir, "..", "..", "..", "..");
//            var debugDll1 = System.IO.Path.Combine(projectRoot, "CurvaLauncher.Plugin.RunApplication", "bin", "Debug", "net6.0-windows");
//            var debugDll2 = System.IO.Path.Combine(projectRoot, "CurvaLauncher.Plugin.RunProgram", "bin", "Debug", "net6.0-windows");

//            foreach (var debugDll in Directory.GetFiles(debugDll1, "*.dll").Concat(Directory.GetFiles(debugDll2, "*.dll")))
//                if (LoadPlugin(debugDll, out CurvaLauncherPluginInstance? plugin))
//                    plugins.Add(plugin);
//#endif

        foreach (FileInfo dllFile in dllFiles)
            if (LoadPlugin(dllFile.FullName, out CurvaLauncherPluginInstance? plugin))
            {
                if (config != null && config.TryGetPropertyValue(plugin.Plugin.GetType().FullName!, out var json))
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

    private bool LoadPlugin(string dllFilePath, [NotNullWhen(true)] out CurvaLauncherPluginInstance? plugin)
    {
        plugin = null;

        try
        {
            var assembly = Assembly.LoadFile(dllFilePath);

            Type? pluginType = assembly.ExportedTypes
                .Where(type => type.IsAssignableTo(typeof(ISyncPlugin)))
                .FirstOrDefault();

            if (pluginType == null)
                return false;

            if (CurvaLauncherPluginInstance.TryCreate(pluginType, out plugin))
                return true;

            return false;
        }
        catch
        {
            return false;
        }
    }

    [RelayCommand]
    public async Task ReloadAllPlugins()
    {
        foreach (var plugin in PluginInstances)
        {
            if (plugin is ISyncPlugin syncPlugin)
                syncPlugin.Init();

            if (plugin is IAsyncPlugin asyncPlugin)
                await asyncPlugin.InitAsync();
        }
    }
}