using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Apis;
using CurvaLauncher.Utilities;
using CurvaLauncher.ViewModels;

namespace CurvaLauncher.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsAboutPage.xaml
    /// </summary>
    public partial class SettingsAboutPage : Wpf.Ui.Controls.UiPage
    {
        public SettingsAboutPage(
            SettingsAboutViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        public SettingsAboutViewModel ViewModel { get; }


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
    }
}
