using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using CurvaLauncher.Plugins;
using CurvaLauncher.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace CurvaLauncher.Services;

public partial class ConfigService : ObservableObject
{
    public string Path { get; set; } = "AppConfig.json";

    private readonly IServiceProvider _serviceProvider;
    private readonly PathService _pathService;

    public ConfigService(
        IServiceProvider serviceProvider,
        PathService pathService)
    {
        _serviceProvider = serviceProvider;
        _pathService = pathService;

        LoadConfig(out var config);
        Config = config;
    }



    [ObservableProperty]
    private AppConfig _config;

    private void LoadConfig(out AppConfig config)
    {
        string fullPath = _pathService.GetPath(Path);
        if (!File.Exists(fullPath))
        {
            config = new AppConfig();
            using FileStream _fs = File.Create(fullPath);
            JsonSerializer.Serialize(_fs, config, JsonUtils.Options);
            return;
        }

        using FileStream fs = File.OpenRead(fullPath);
        config = JsonSerializer.Deserialize<AppConfig>(fs, JsonUtils.Options) ?? new AppConfig();
    }

    private AppConfig.PluginConfig GetPluginConfig(CurvaLauncherPluginInstance pluginInstance)
    {
        return new()
        {
            IsEnabled = pluginInstance.IsEnabled,
            Weight = pluginInstance.Weight,
            Options = GetPluginOptions(pluginInstance.Plugin)
        };

        JsonObject GetPluginOptions(IPlugin plugin)
        {
            var props = plugin.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttribute<PluginOptionAttribute>() is not null);

            JsonObject json = new();
            foreach (var property in props)
            {
                json[property.Name] = JsonSerializer.SerializeToNode(property.GetValue(pluginInstance.Plugin));
            }

            return json;
        }
    }

    [RelayCommand]
    public void Load()
    {
        LoadConfig(out var config);
        Config = config;
    }

    [RelayCommand]
    public void Save()
    {
        var pluginService = _serviceProvider
            .GetRequiredService<PluginService>();

        Config.Plugins = pluginService.PluginInstances
            .ToDictionary(instance => instance.Plugin.GetType().FullName!, instance => GetPluginConfig(instance));

        string fullPath = _pathService.GetPath(Path);
        using FileStream fs = File.Create(fullPath);
        JsonSerializer.Serialize(fs, Config, JsonUtils.Options);
    }
}
