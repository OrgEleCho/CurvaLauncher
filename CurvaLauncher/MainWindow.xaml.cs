using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CurvaLauncher.Services;
using CurvaLauncher.ViewModels;
using Wpf.Ui.Appearance;

namespace CurvaLauncher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[ObservableObject]
public partial class MainWindow : Wpf.Ui.Controls.UiWindow
{
    public MainWindow(
        MainViewModel viewModel,
        ConfigService configService)
    {
        DataContext = this;
        AppConfig = configService.Config;
        ViewModel = viewModel;

        InitializeComponent();
    }

    public AppConfig AppConfig { get; }
    public MainViewModel ViewModel { get; }


    [ObservableProperty]
    private bool _isHotkeyRegisterSucceed;

    private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        App.CloseLauncher();
    }

    private void Window_Deactivated(object sender, EventArgs e)
    {
        if (AppConfig.KeepLauncherWhenFocusLost)
            return;

//#if RELEASE
        App.CloseLauncher();
//#endif
    }

    public void Clear()
    {
        ViewModel.QueryText = string.Empty;

    }

    private void UiWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Wpf.Ui.Appearance.Watcher.Watch(this, BackgroundType.Mica, true);
    }

    private void NotifyIcon_LeftClick(Wpf.Ui.Controls.NotifyIcon sender, RoutedEventArgs e)
    {
        App.ShowLauncher();
    }
}
