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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicaLauncher.Services;
using MicaLauncher.ViewModels;

namespace MicaLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ObservableObject]
    public partial class MainWindow : Window
    {
        public MainWindow(
            MainViewModel viewModel,
            ConfigService configService)
        {
            InitializeComponent();

            AppConfig = configService.Config;
            ViewModel = viewModel;
            DataContext = this;
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

            App.CloseLauncher();
        }
    }
}
