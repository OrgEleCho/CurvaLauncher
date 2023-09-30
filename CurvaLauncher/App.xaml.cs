using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities;
using CurvaLauncher.View;
using CurvaLauncher.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CurvaLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; } = BuildServiceProvider();

        private static IServiceProvider BuildServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsWindow>();
            services.AddSingleton<SettingsViewModel>();

            services.AddSingleton<PathService>();
            services.AddSingleton<HotkeyService>();
            services.AddSingleton<PluginService>();
            services.AddSingleton<ConfigService>();

            return services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!EnsureAppSingleton())
            {
                Shutdown();
                return;
            }

            base.OnStartup(e);

            var hotkeyService = ServiceProvider
                .GetRequiredService<HotkeyService>();

            // 初始化热键
            hotkeyService.Register();

            // 创建窗口
            ServiceProvider
                .GetRequiredService<MainWindow>();

            if (!hotkeyService.Registered)
            {
                NativeMethods.MessageBox(IntPtr.Zero, $"CurvaLauncher hotkey registration failed. Please check if there are other programs occupying the hotkey and restart CurvaLauncher.", "CurvaLauncher Error", MessageBoxFlags.Ok | MessageBoxFlags.IconError);
                Application.Current.Shutdown();
            }            
        }

        private bool EnsureAppSingleton()
        {
            EventWaitHandle singletonEvent =
                new EventWaitHandle(false, EventResetMode.AutoReset, "SlimeNull/CurvaLauncher", out bool createdNew);

            if (createdNew)
            {
                // 启动一个等待信号的 Task
                Task.Run(() =>
                {
                    while (true)
                    {
                        // 等待一个信号
                        singletonEvent.WaitOne();

                        Dispatcher.Invoke(() =>
                        {
                            ShowLauncher();
                        });
                    }
                });

                return true;
            }
            else
            {
                // 向已经存在的程序发送一个信号
                singletonEvent.Set();
                return false;
            }
        }

        public static RelayCommand ShowLauncherCommand { get; }
            = new RelayCommand(ShowLauncher);
        public static RelayCommand CloseLauncherCommand { get; }
            = new RelayCommand(CloseLauncher);
        public static RelayCommand ShowLauncherSettingsCommand { get; }
            = new RelayCommand(ShowLauncherSettings);

        public static RelayCommand ShutdownCommand { get; }
            = new RelayCommand(Application.Current.Shutdown);


        public static void ShowLauncher()
        {
            var mainWindow =
                ServiceProvider.GetRequiredService<MainWindow>();
            var configService =
                ServiceProvider.GetRequiredService<ConfigService>();

            mainWindow.Width = configService.Config.LauncherWidth;
            mainWindow.Left = (SystemParameters.PrimaryScreenWidth - mainWindow.AppConfig.LauncherWidth) / 2;
            mainWindow.Top = SystemParameters.PrimaryScreenHeight / 3;

            mainWindow.Show();
            mainWindow.Activate();
            mainWindow.QueryBox.Focus();
        }

        public static void CloseLauncher()
        {
            var mainWindow = 
                ServiceProvider.GetRequiredService<MainWindow>();

            mainWindow.ViewModel.QueryText = string.Empty;
            mainWindow.Visibility = Visibility.Collapsed;
        }

        public static void ShowLauncherSettings()
        {
            SettingsWindow settingsWindow = 
                ServiceProvider.GetRequiredService<SettingsWindow>();

            settingsWindow.WindowState = WindowState.Normal;
            settingsWindow.Show();
            settingsWindow.Activate();
        }
    }
}
