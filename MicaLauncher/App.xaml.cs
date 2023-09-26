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
using MicaLauncher.Services;
using MicaLauncher.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MicaLauncher
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

            services.AddSingleton<PathService>();
            services.AddSingleton<PluginService>();

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

            // 创建窗口
            ServiceProvider.GetRequiredService<MainWindow>();
        }

        private bool EnsureAppSingleton()
        {
            EventWaitHandle singletonEvent =
                new EventWaitHandle(false, EventResetMode.AutoReset, "SlimeNull/MicaLauncher", out bool createdNew);

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

        public static RelayCommand CloseLauncherCommand { get; }
            = new RelayCommand(CloseLauncher);
        public static RelayCommand ShowLauncherCommand { get; }
            = new RelayCommand(ShowLauncher);

        public static void CloseLauncher()
        {
            var mainWindow = 
                ServiceProvider.GetRequiredService<MainWindow>();

            mainWindow.ViewModel.QueryText = string.Empty;
            mainWindow.Visibility = Visibility.Collapsed;
        }

        public static void ShowLauncher()
        {
            var mainWindow =
                ServiceProvider.GetRequiredService<MainWindow>();

            mainWindow.Show();
            mainWindow.Activate();
            mainWindow.QueryBox.Focus();
        }
    }
}
