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
using System.Windows.Shapes;
using MicaLauncher.Services;
using MicaLauncher.ViewModels;

namespace MicaLauncher.View
{
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
}
