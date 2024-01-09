using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Libraries.Securify.ShellLink;
using CurvaLauncher.Models;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities;
using CurvaLauncher.ViewModels;
using CurvaLauncher.Views.Components;
using Microsoft.Win32;
using Wpf.Ui.Appearance;
using Wpf.Ui.Mvvm.Services;
using WpfMsgBox = Wpf.Ui.Controls.MessageBox;

namespace CurvaLauncher.Views;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
[ObservableObject]
public partial class SettingsWindow : Wpf.Ui.Controls.UiWindow
{
    private readonly Dictionary<CurvaLauncherPluginInstance, PluginOptionsControl> _cachedPluginOptions = new();

    public SettingsWindow(
        SettingsViewModel viewModel,
        ConfigService configService,
        PluginService pluginService,
        HotkeyService hotkeyService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        ConfigService = configService;
        PluginService = pluginService;
        HotkeyService = hotkeyService;
        DataContext = this;

        ViewModel.SelectedPluginInstance =
            PluginService.PluginInstances.FirstOrDefault();
        NavigateToPluginSettings(ViewModel.SelectedPluginInstance);
    }

    public SettingsViewModel ViewModel { get; }
    public ConfigService ConfigService { get; }
    public PluginService PluginService { get; }
    public HotkeyService HotkeyService { get; }


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
            _cachedPluginOptions[pluginInstance] = pluginOption = new PluginOptionsControl(pluginInstance);

        pluginOptionContainer.Content = pluginOption;
    }

    [RelayCommand]
    public void CheckHotkeyStatus()
    {
        var hotkey = ConfigService.Config.LauncherHotkey;

        ViewModel.HotkeyNotValid = !HotkeyService.IsValidHotkey();
        ViewModel.HotkeyNotRegistered = !HotkeyService.Registered;

        ViewModel.ShowHotkeyApplyButton = !ViewModel.HotkeyNotValid && (!HotkeyService.Registered || HotkeyService.RegisteredHotkey != hotkey);
    }

    [RelayCommand]
    public void ApplyHotkey()
    {
        HotkeyService.Register();
        CheckHotkeyStatus();

        if (!HotkeyService.Registered)
        {
            NativeMethods.MessageBox(
                IntPtr.Zero,
                $"CurvaLauncher hotkey registration failed. Please check if there are other programs occupying the hotkey and restart CurvaLauncher.",
                "CurvaLauncher Warning",
                MessageBoxFlags.Ok | MessageBoxFlags.IconWarning);
        }
    }


    [RelayCommand]
    public void CheckStartupStatus()
    {
        var selfPath = System.Environment.ProcessPath!;
        var runDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        var path = Path.Combine(runDir, $"{nameof(CurvaLauncher)}.lnk");

        try
        {
            if (File.Exists(path))
            {
                var rawBytes = File.ReadAllBytes(path);
                var shortcut = Shortcut.FromByteArray(rawBytes);

                if (shortcut.ExtraData.EnvironmentVariableDataBlock.TargetUnicode.Equals(selfPath, StringComparison.OrdinalIgnoreCase))
                    ConfigService.Config.StartsWithWindows = true;
                else
                    ConfigService.Config.StartsWithWindows = false;
            }
            else
            {
                ConfigService.Config.StartsWithWindows = false;
            }
        }
        catch { }
    }

    [RelayCommand]
    public void SwitchOnStartup()
    {
        try
        {
            var runDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var shortcut = Shortcut.CreateShortcut( System.Environment.ProcessPath!);
            var path = Path.Combine(runDir, $"{nameof(CurvaLauncher)}.lnk");

            File.WriteAllBytes(path, shortcut.GetBytes());
            ConfigService.Config.StartsWithWindows = true;
        }
        catch (Exception)
        {
            MessageBox.Show("Failed to create startup shortcut", "CurvaLauncher Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ConfigService.Config.StartsWithWindows = false;
        }
    }

    [RelayCommand]
    public void SwitchOffStartup()
    {
        try
        {
            var runDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            var path = Path.Combine(runDir, $"{nameof(CurvaLauncher)}.lnk");

            if (File.Exists(path))
                File.Delete(path);

            ConfigService.Config.StartsWithWindows = false;
        }
        catch (Exception)
        {
            MessageBox.Show("Failed to create startup shortcut", "CurvaLauncher Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ConfigService.Config.StartsWithWindows = true;
        }
    }

    [RelayCommand]
    public async Task CheckVersion()
    {
        try
        {
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version!;
            var latestVersion = await GithubUtils.GetLatestVersionAsync(default);

            if (latestVersion != null && latestVersion.Value.Version > currentVersion)
            {
                if (MessageBox.Show($"New version found: {latestVersion.Value.Version}, would you want to go and download it?", "CurvaLauncher Tips", MessageBoxButton.YesNo, MessageBoxImage.Information) == System.Windows.MessageBoxResult.Yes)
                {
                    ShellUtils.Start(latestVersion.Value.Address);
                }
            }
            else
            {
                MessageBox.Show("No update", "CurvaLauncher Tips", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch
        {
            MessageBox.Show("Failed to check update", "CurvaLauncher Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    [RelayCommand]
    public void SaveSettings()
    {
        ConfigService.Save();
    }
}
