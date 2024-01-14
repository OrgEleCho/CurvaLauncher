using System;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

    private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        App.CloseLauncher();
    }

    private void WindowDeactivated(object sender, EventArgs e)
    {
        if (AppConfig.KeepLauncherWhenFocusLost)
            return;

        App.CloseLauncher();
    }

    private void UiWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Wpf.Ui.Appearance.Watcher.Watch(this, BackgroundType.Mica, true);
    }


    private void QueryBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Up &&
            string.IsNullOrWhiteSpace(ViewModel.QueryText) &&
            ViewModel.LastInvokedQueryText is string lastInvokedQueryText)
        {
            SetQueryText(lastInvokedQueryText);
            e.Handled = true;
        }
    }

    [RelayCommand]
    public void ScrollToSelectedQueryResult()
    {
        resultBox.ScrollIntoView(resultBox.SelectedItem);
    }

    public void SetQueryText(string text)
    {
        QueryBox.Text = text;
        QueryBox.SelectionStart = text.Length;
        QueryBox.SelectionLength = 0;
    }
}
