using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using CurvaLauncher.Plugins;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.Loader;
using IOPath = System.IO.Path;

namespace CurvaLauncher.Services;

public partial class PluginService
{
    private readonly PathService _pathService;
    private readonly ConfigService _configService;

    public string Path { get; set; } = "Plugins";

    public ObservableCollection<PluginInstance> PluginInstances { get; } = new();

    public PluginService(
        PathService pathService,
        ConfigService configService)
    {
        _pathService = pathService;
        _configService = configService;
    }


    private DirectoryInfo EnsurePluginDirectory()
    {
        DirectoryInfo dir = new(_pathService.GetPath(Path));

        if (!dir.Exists)
            dir.Create();

        return dir;
    }

    private void CoreLoadPlugins(out List<PluginInstance> plugins)
    {
        plugins = new List<PluginInstance>();

        var dir = EnsurePluginDirectory();

        AppConfig config = _configService.Config;

        foreach (var dllFile in dir.GetFiles("*.dll"))
        {
            if (CoreLoadDllPlugin(config, dllFile.FullName, out PluginInstance? plugin))
            {
                plugins.Add(plugin);
            }
        }

        foreach (var zipFile in dir.GetFiles("*.zip"))
        {
            if (CoreLoadZipPlugin(config, zipFile.FullName, out PluginInstance? plugin))
            {
                plugins.Add(plugin);
            }
        }
    }

    private bool CoreLoadPluginFromAssembly(AppConfig config, Assembly assembly, [NotNullWhen(true)] out PluginInstance? pluginInstance)
    {
        pluginInstance = null;

        Type? pluginType = assembly.ExportedTypes
                .Where(type => type.IsAssignableTo(typeof(ISyncPlugin)) || type.IsAssignableTo(typeof(IAsyncPlugin)))
                .FirstOrDefault();

        if (pluginType == null)
            return false;

        if (!PluginInstance.TryCreate(pluginType, out pluginInstance))
            return false;

        var typeName = pluginType.FullName!;

        if (config.Plugins.TryGetValue(typeName, out var pluginConfig))
        {
            var props = pluginInstance.Plugin.GetType().GetProperties()
                        .Where(p => p.GetCustomAttribute<PluginOptionAttribute>() is not null
                            || p.GetCustomAttribute<PluginI18nOptionAttribute>() is not null);

            if (pluginConfig.Options != null)
            {
                foreach (var property in props)
                {
                    if (pluginConfig.Options.TryGetPropertyValue(property.Name, out var value))
                    {
                        var type = property.PropertyType;
                        var val = JsonSerializer.Deserialize(value, type);
                        property.SetValue(pluginInstance.Plugin, val);
                    }
                }
            }

            pluginInstance.IsEnabled = pluginConfig.IsEnabled;
            pluginInstance.Weight = pluginConfig.Weight;
        }
        else
        {
            pluginInstance.IsEnabled = true;
        }

        return true;
    }

    private bool CoreLoadDllPlugin(AppConfig config, string dllFilePath, [NotNullWhen(true)] out PluginInstance? pluginInstance)
    {
        pluginInstance = null;

        try
        {
            var assembly = Assembly.LoadFile(dllFilePath);
            return CoreLoadPluginFromAssembly(config, assembly, out pluginInstance);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Plugin load failed, {ex}");
            return false;
        }
    }

    private bool CoreLoadZipPlugin(AppConfig config, string zipFilePath, [NotNullWhen(true)] out PluginInstance? pluginInstance)
    {
        pluginInstance = null;

        try
        {
            using var zipFile = File.OpenRead(zipFilePath);
            string extractDir = IOPath.Combine(".cache", IOPath.GetFileNameWithoutExtension(zipFilePath));
            if (Directory.Exists(extractDir))
                Directory.Delete(extractDir, true);
            ZipFile.ExtractToDirectory(zipFile, extractDir);

            var manifestJson = File.ReadAllText(IOPath.Combine(extractDir, "Manifest.json"));
            var manifest = JsonSerializer.Deserialize<PluginManifest>(manifestJson);
            if (manifest is null)
                return false;

            var assemblyPath = IOPath.GetFullPath(IOPath.Combine(extractDir, manifest.Assembly));
            var alc = new PluginAssemblyLoadContext(manifest.ID, assemblyPath);
            Assembly assembly = alc.LoadFromAssemblyPath(assemblyPath);
            return CoreLoadPluginFromAssembly(config, assembly, out pluginInstance);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Plugin load failed, {ex}");
            return false;
        }
    }

    private void MoveCommandPluginsToTheBeginning(IList<PluginInstance> plugins)
    {
        int indexStart = 0;
        for (int i = 1; i < plugins.Count; i++)
        {
            if (plugins[i].Plugin is CommandPlugin)
            {
                (plugins[indexStart], plugins[i]) = (plugins[i], plugins[indexStart]);
                indexStart++;
            }
        }
    }

    [RelayCommand]
    public void LoadAllPlugins()
    {
        CoreLoadPlugins(out var plugins);
        MoveCommandPluginsToTheBeginning(plugins);

        PluginInstances.Clear();
        foreach (var plugin in plugins)
            PluginInstances.Add(plugin);
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

    [RelayCommand]
    public async Task FinishAllPlugins()
    {
        foreach (var plugin in PluginInstances.Where(ins => ins.IsEnabled))
        {
            if (plugin.Plugin is ISyncPlugin syncPlugin)
                syncPlugin.Finish();
            else if (plugin.Plugin is IAsyncPlugin asyncPlugin)
                await asyncPlugin.FinishAsync();
        }
    }
}