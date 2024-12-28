using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
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

    private void WindowLoaded(object sender, RoutedEventArgs e)
    {
        Wpf.Ui.Appearance.Watcher.Watch(this, BackgroundType.Mica, true);

        Width = AppConfig.LauncherWidth;
        Left = (SystemParameters.PrimaryScreenWidth - AppConfig.LauncherWidth) / 2;
        Top = SystemParameters.PrimaryScreenHeight / 4;
    }

    private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        App.CloseLauncher();
    }

    private void WindowActivated(object sender, EventArgs e)
    {
        FocusManager.SetFocusedElement(this, QueryBox);
        ViewModel.QueryCommand.Execute(null);
        Focus();
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
}
