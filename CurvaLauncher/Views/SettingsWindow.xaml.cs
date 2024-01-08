using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using CurvaLauncher.Services;
using CurvaLauncher.ViewModels;
using CurvaLauncher.Views.Components;

namespace CurvaLauncher.Views;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    private readonly Dictionary<CurvaLauncherPluginInstance, PluginOptionsControl> _cachedPluginOptions = new();

    public SettingsWindow(
        SettingsViewModel viewModel,
        ConfigService configService,
        PluginService pluginService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        ConfigService = configService;
        PluginService = pluginService;
        DataContext = this;

        ViewModel.SelectedPluginInstance = 
            PluginService.PluginInstances.FirstOrDefault();
        NavigateToPluginSettings(ViewModel.SelectedPluginInstance);
    }

    public SettingsViewModel ViewModel { get; }
    public ConfigService ConfigService { get; }
    public PluginService PluginService { get; }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }

    [RelayCommand]
    public void CloseSettings()
    {
        Hide();
    }

    [RelayCommand]
    public void NavigateToPluginSettings(CurvaLauncherPluginInstance? pluginInstance)
    {
        if (pluginInstance is null)
            return;

        if (!_cachedPluginOptions.TryGetValue(pluginInstance, out var pluginOption))
            _cachedPluginOptions[pluginInstance] = pluginOption = new PluginOptionsControl(pluginInstance.Plugin);

        pluginOptionContainer.Content = pluginOption;
    }

}
