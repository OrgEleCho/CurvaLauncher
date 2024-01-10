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
using CurvaLauncher.Views;
using CurvaLauncher.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using System.Reflection;
using System.Diagnostics;
using CurvaLauncher.Views.Pages;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

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

            // view
            services.AddSingleton<MainWindow>();
            services.AddSingleton<SettingsWindow>();
            services.AddSingleton<SettingsGeneralPage>();
            services.AddSingleton<SettingsPluginPage>();
            services.AddSingleton<SettingsHotkeyPage>();
            services.AddSingleton<SettingsAboutPage>();

            // view model
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<SettingsGeneralViewModel>();
            services.AddSingleton<SettingsPluginViewModel>();
            services.AddSingleton<SettingsHotkeyViewModel>();
            services.AddSingleton<SettingsAboutViewModel>();

            // services
            services.AddSingleton<PathService>();
            services.AddSingleton<ConfigService>();
            services.AddSingleton<HotkeyService>();
            services.AddSingleton<PluginService>();
            services.AddSingleton<GlobalizationService>();
            services.AddSingleton<PageService>();

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
            var globalizationService = ServiceProvider
                .GetRequiredService<GlobalizationService>();

            // 初始化热键
            hotkeyService.Register();

            // 初始化语言
            globalizationService.ApplyLanguage();

            // 创建窗口
            ServiceProvider
                .GetRequiredService<MainWindow>();

            if (!hotkeyService.IsLauncherHotkeyRegistered)
            {
                NativeMethods.MessageBox(
                    IntPtr.Zero,
                    $"CurvaLauncher hotkey registration failed. Please check if there are other programs occupying the hotkey and restart CurvaLauncher.",
                    "CurvaLauncher Warning",
                    MessageBoxFlags.Ok | MessageBoxFlags.IconWarning);
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
        public static IRelayCommand ShellStartCommand { get; }
            = new RelayCommand<string>(ShellUtils.Start);

        public static RelayCommand ShutdownCommand { get; }
            = new RelayCommand(Application.Current.Shutdown);

        private static CancellationTokenSource _currentCancellationTokenSource = new();

        public static CancellationToken GetLauncherCancellationToken() => _currentCancellationTokenSource.Token;

        public static void ShowLauncher()
        {
            _currentCancellationTokenSource = new();

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

        public static void ShowLauncherWithQuery(string queryText)
        {
            _currentCancellationTokenSource = new();

            var mainWindow =
                ServiceProvider.GetRequiredService<MainWindow>();
            var configService =
                ServiceProvider.GetRequiredService<ConfigService>();

            mainWindow.Width = configService.Config.LauncherWidth;
            mainWindow.Left = (SystemParameters.PrimaryScreenWidth - mainWindow.AppConfig.LauncherWidth) / 2;
            mainWindow.Top = SystemParameters.PrimaryScreenHeight / 3;

            mainWindow.Show();
            mainWindow.Activate();
            mainWindow.SetQueryText(queryText);
            mainWindow.QueryBox.Focus();
        }

        public static void CloseLauncher()
        {
            var mainWindow = 
                ServiceProvider.GetRequiredService<MainWindow>();

            mainWindow.ViewModel.QueryText = string.Empty;
            mainWindow.ViewModel.QueryResults.Clear();
            mainWindow.Hide();

            _currentCancellationTokenSource.Cancel();
        }

        public static void ShowLauncherSettings()
        {
            SettingsWindow settingsWindow = 
                ServiceProvider.GetRequiredService<SettingsWindow>();

            settingsWindow.WindowState = WindowState.Normal;
            settingsWindow.Show();
            settingsWindow.Activate();
        }

        public static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "[ Unknown Version ]";

        public static string RepositoryAddress { get; } = "https://github.com/OrgEleCho/CurvaLauncher";
    }
}
