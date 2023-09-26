using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicaLauncher.Utilities;

namespace MicaLauncher.Services
{
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
}
