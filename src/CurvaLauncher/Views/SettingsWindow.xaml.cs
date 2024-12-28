using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CurvaLauncher.Views.Pages;
using Microsoft.Win32;
using Wpf.Ui.Appearance;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace CurvaLauncher.Views;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
[ObservableObject]
public partial class SettingsWindow : Wpf.Ui.Controls.UiWindow
{
    public static SettingsWindow? Existed { get; private set; }

    public SettingsWindow(
        SettingsViewModel viewModel,
        ConfigService configService,
        PageService pageService)
    {
        ViewModel = viewModel;
        ConfigService = configService;
        DataContext = this;

        InitializeComponent();

        navigationStore.PageService = pageService;

        Existed = this;
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Existed = null;
    }

    public SettingsViewModel ViewModel { get; }
    public ConfigService ConfigService { get; }


    private void NavigationStoreLoaded(object sender, RoutedEventArgs e)
    {
        bool ok = navigationStore.Navigate("General");
    }

    [RelayCommand]
    public void CloseSettings()
    {
        Close();
    }

    [RelayCommand]
    public void ApplySettings()
    {
        ConfigService.Save();
    }

    [RelayCommand]
    public void ApplyAndCloseSettings()
    {
        ConfigService.Save();
        Close();
    }
}
