using System;
using System.Collections.Generic;
using System.Linq;
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
using CurvaLauncher.Models;
using CurvaLauncher.Services;
using CurvaLauncher.ViewModels;
using CurvaLauncher.Views.Components;

namespace CurvaLauncher.Views.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPluginPage.xaml
    /// </summary>
    public partial class SettingsPluginPage : Wpf.Ui.Controls.UiPage
    {
        private readonly Dictionary<CurvaLauncherPluginInstance, PluginOptionsControl> _cachedPluginOptions = new();

        public SettingsPluginPage(
            SettingsPluginViewModel viewModel,
            PluginService pluginService)
        {
            ViewModel = viewModel;
            PluginService = pluginService;
            DataContext = this;

            InitializeComponent();

            ViewModel.SelectedPluginInstance =
                PluginService.PluginInstances.FirstOrDefault();
                NavigateToPluginSettings(ViewModel.SelectedPluginInstance);
        }

        public SettingsPluginViewModel ViewModel { get; }
        public PluginService PluginService { get; }


        [RelayCommand]
        public void NavigateToPluginSettings(CurvaLauncherPluginInstance? pluginInstance)
        {
            if (pluginInstance is null)
                return;

            if (!_cachedPluginOptions.TryGetValue(pluginInstance, out var pluginOption))
                _cachedPluginOptions[pluginInstance] = pluginOption = new PluginOptionsControl(pluginInstance);

            pluginOptionContainer.Content = pluginOption;
        }
    }
}
