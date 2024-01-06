using System.Windows;
using CurvaLauncher.Services;
using CurvaLauncher.ViewModels;

namespace CurvaLauncher.Views;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow(
        SettingsViewModel viewModel,
        ConfigService configService)
    {
        InitializeComponent();

        ViewModel = viewModel;
        ConfigService = configService;
        DataContext = this;
    }

    public SettingsViewModel ViewModel { get; }
    public ConfigService ConfigService { get; }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
    }
}
