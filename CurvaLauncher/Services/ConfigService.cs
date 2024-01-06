using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Services;

public partial class ConfigService : ObservableObject
{
    public string Path { get; set; } = "AppConfig.json";

    private readonly PathService _pathService;

    public ConfigService(
        PathService pathService)
    {
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



    [RelayCommand]
    public void Load()
    {
        LoadConfig(out var config);
        Config = config;
    }

    [RelayCommand]
    public void Save()
    {
        string fullPath = _pathService.GetPath(Path);
        using FileStream fs = File.Create(fullPath);
        JsonSerializer.Serialize(fs, Config, JsonUtils.Options);
    }
}
