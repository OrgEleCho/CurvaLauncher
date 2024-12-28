using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities;
using CurvaLauncher.ViewModels;
using Wpf.Ui.Appearance;

namespace CurvaLauncher;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[ObservableObject]
public partial class MainWindow : Wpf.Ui.Controls.UiWindow
{
    private bool _firstShow = true;

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

    private void InitializeWindowRect()
    {
        var targetScreen = System.Windows.Forms.Screen.PrimaryScreen;

        if (AppConfig.WindowStartupScreen == WindowStartupScreen.ScreenWithMouse)
        {
            var drawingMousePosition = System.Windows.Forms.Control.MousePosition;

            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen.WorkingArea.Contains(drawingMousePosition))
                {
                    targetScreen = screen;
                    break;
                }
            }
        }

        if (targetScreen is null)
        {
            return;
        }

        var windowInteropHelper = new WindowInteropHelper(this);
        var pixelLauncherWidth = (int)(AppConfig.LauncherWidth * VisualTreeHelper.GetDpi(this).DpiScaleX);

        NativeMethods.GetWindowRect(windowInteropHelper.Handle, out var windowRect);
        NativeMethods.SetWindowPos(windowInteropHelper.Handle, 0,
            targetScreen.WorkingArea.Left + (targetScreen.WorkingArea.Width - pixelLauncherWidth) / 2,
            targetScreen.WorkingArea.Top + (targetScreen.WorkingArea.Height / 4),
            pixelLauncherWidth,
            windowRect.Height,
            0x0001);
    }

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        Wpf.Ui.Appearance.Watcher.Watch(this, BackgroundType.Mica, true);

        InitializeWindowRect();
    }

    private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        App.CloseLauncher();
    }

    private void WindowDeactivated(object sender, EventArgs e)
    {
        if (AppConfig.KeepLauncherWhenFocusLost)
            return;

        ViewModel.QueryResults.Clear();
        ViewModel.SelectedQueryResult = null;

        ResultBox.SelectedItem = null;

        App.CloseLauncher();
    }


    private void QueryBox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (ViewModel.ImmediateResults.Count != 0)
        {
            return;
        }

        if (e.Key == Key.Up &&
            string.IsNullOrWhiteSpace(ViewModel.QueryText) &&
            !string.IsNullOrWhiteSpace(ViewModel.LastInvokedQueryText) &&
            ViewModel.LastInvokedQueryText is string lastInvokedQueryText)
        {
            SetQueryText(lastInvokedQueryText);
            e.Handled = true;
        }
    }

    [RelayCommand]
    public void ScrollToSelectedItem(ListView? listView)
    {
        if (listView is null)
        {
            return;
        }

        if (listView.SelectedItem is null ||
            listView.SelectedIndex < 0)
        {
            return;
        }

        try
        {
            listView.ScrollIntoView(listView.SelectedItem);
        }
        catch { }
    }

    public void SetQueryText(string text)
    {
        QueryBox.Text = text;
        QueryBox.SelectionStart = text.Length;
        QueryBox.SelectionLength = 0;
    }

    private void ResultBoxItemMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListBoxItem lbi ||
            lbi.Content is not QueryResultModel queryResult)
        {
            return;
        }

        ViewModel.InvokeCommand.Execute(queryResult);
    }

    private void WindowIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (IsVisible)
        {
            if (!_firstShow)
            {
                InitializeWindowRect();
            }

            ViewModel.QueryCommand.Execute(null);
            _firstShow = false;
        }
    }
}
