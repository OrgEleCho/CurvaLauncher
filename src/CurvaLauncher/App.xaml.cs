﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CurvaLauncher.Apis;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities;
using CurvaLauncher.ViewModels;
using CurvaLauncher.Views;
using CurvaLauncher.Views.Pages;
using Microsoft.Extensions.DependencyInjection;
using Wpf.Ui.Controls;

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
            services.AddTransient<SettingsWindow>();
            services.AddScoped<SettingsGeneralPage>();
            services.AddScoped<SettingsPluginPage>();
            services.AddScoped<SettingsHotkeyPage>();
            services.AddScoped<SettingsAboutPage>();

            // view model
            services.AddSingleton<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsGeneralViewModel>();
            services.AddTransient<SettingsPluginViewModel>();
            services.AddTransient<SettingsHotkeyViewModel>();
            services.AddTransient<SettingsAboutViewModel>();

            // services
            services.AddSingleton<PathService>();
            services.AddSingleton<ConfigService>();
            services.AddSingleton<HotkeyService>();
            services.AddSingleton<PluginService>();
            services.AddSingleton<ConfigService>();
            services.AddSingleton<ThemeService>();
            services.AddSingleton<I18nService>();
            services.AddTransient<PageService>();

            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

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

            // 获取所需服务
            var mainWindow = ServiceProvider
                .GetRequiredService<MainWindow>();
            var pluginService = ServiceProvider
                .GetRequiredService<PluginService>();
            var hotkeyService = ServiceProvider
                .GetRequiredService<HotkeyService>();
            var themeService = ServiceProvider
                .GetRequiredService<ThemeService>();
            var globalizationService = ServiceProvider
                .GetRequiredService<I18nService>();


            //// 初始化窗口
            //new WindowInteropHelper(mainWindow)
            //    .EnsureHandle();

            // 加载插件
            pluginService.LoadAllPlugins();

            // 初始化热键
            hotkeyService.RegisterAll();

            // 初始化语言
            globalizationService.ApplyLanguage();

            // 创建窗口
            ServiceProvider
                .GetRequiredService<MainWindow>();

            // 初始化主题
            themeService.ApplyTheme();

            // 初始化托盘图标
            InitializeNotifyIcon(mainWindow);

            if (!hotkeyService.IsLauncherHotkeyRegistered)
            {
                NativeMethods.MessageBox(
                    IntPtr.Zero,
                    $"CurvaLauncher hotkey registration failed. Please check if there are other programs occupying the hotkey and restart CurvaLauncher.",
                    "CurvaLauncher Warning",
                    MessageBoxFlags.Ok | MessageBoxFlags.IconWarning);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _ = App.ServiceProvider
                .GetRequiredService<PluginService>()
                .FinishAllPlugins();

            base.OnExit(e);
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
            mainWindow.Top = SystemParameters.PrimaryScreenHeight / 4;

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
            SettingsWindow settingsWindow = SettingsWindow.Existed ??
                ServiceProvider.GetRequiredService<SettingsWindow>();

            settingsWindow.WindowState = WindowState.Normal;
            settingsWindow.Show();
            settingsWindow.Activate();
        }

        public static void InitializeNotifyIcon(MainWindow window)
        {
            var hwnd = new WindowInteropHelper(window)
                .EnsureHandle();

            var wpfuiAssembly = typeof(NotifyIcon).Assembly;
            var trayManagerType = wpfuiAssembly.GetType("Wpf.Ui.Tray.TrayManager")!;
            var iNotifyIconType = wpfuiAssembly.GetType("Wpf.Ui.Tray.INotifyIcon")!;
            var notifyIconType = window.notifyIcon.GetType();

            var registerMethod = trayManagerType.GetMethod("Register", (BindingFlags)(-1), [iNotifyIconType, typeof(HwndSource)])!;
            var notifyIconServiceField = notifyIconType.GetField("_notifyIconService", (BindingFlags)(-1))!;

            var notifyIconService = notifyIconServiceField.GetValue(window.notifyIcon);
            var hwndSource = HwndSource.FromHwnd(hwnd);

            NotifyIconInitializeIcon(window.notifyIcon);

            registerMethod.Invoke(null, [notifyIconService, hwndSource]);

            [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "InitializeIcon")]
            static extern void NotifyIconInitializeIcon(NotifyIcon notifyIcon);
        }

        public static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "[ Unknown Version ]";

        public static string RepositoryAddress { get; } = "https://github.com/OrgEleCho/CurvaLauncher";
    }
}
