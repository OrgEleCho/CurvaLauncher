﻿using System;
using System.IO;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Libraries.Securify.ShellLink;
using CurvaLauncher.Models;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities;
using CurvaLauncher.ViewModels;
using Wpf.Ui.Appearance;

namespace CurvaLauncher.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsGeneralPage.xaml
    /// </summary>
    public partial class SettingsGeneralPage : Wpf.Ui.Controls.UiPage
    {
        public SettingsGeneralPage(
            SettingsGeneralViewModel viewModel,
            ConfigService configService,
            HotkeyService hotkeyService,
            ThemeService themeService,
            I18nService i18nService)
        {
            ViewModel = viewModel;
            ConfigService = configService;
            HotkeyService = hotkeyService;
            ThemeService = themeService;
            I18nService = i18nService;
            DataContext = this;

            InitializeComponent();
        }

        public SettingsGeneralViewModel ViewModel { get; }
        public ConfigService ConfigService { get; }
        public HotkeyService HotkeyService { get; }
        public ThemeService ThemeService { get; }
        public I18nService I18nService { get; }

        [RelayCommand]
        public void CheckHotkeyStatus()
        {
            var hotkey = ConfigService.Config.LauncherHotkey;

            ViewModel.HotkeyNotValid = !HotkeyUtils.IsValidHotkey(hotkey);
            ViewModel.HotkeyNotRegistered = !HotkeyService.IsLauncherHotkeyRegistered;

            ViewModel.ShowHotkeyApplyButton = !ViewModel.HotkeyNotValid && (!HotkeyService.IsLauncherHotkeyRegistered || HotkeyService.RegisteredHotkey != hotkey);
        }

        [RelayCommand]
        public void ApplyHotkey()
        {
            HotkeyService.RegisterLauncherHotkey();
            CheckHotkeyStatus();

            if (!HotkeyService.IsLauncherHotkeyRegistered)
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

                    if (shortcut?.ExtraData?.EnvironmentVariableDataBlock?.TargetUnicode?.Equals(selfPath, StringComparison.OrdinalIgnoreCase) ?? false)
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
    }
}
