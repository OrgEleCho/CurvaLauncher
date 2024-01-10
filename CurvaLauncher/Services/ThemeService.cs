using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Windows.Interop;
using Wpf.Ui.Appearance;

namespace CurvaLauncher.Services
{
    public partial class ThemeService
    {
        static readonly Func<SystemThemeType> s_systemThemeGetter;

        static ThemeService()
        {
            var type = typeof(Theme).Assembly.GetType("Wpf.Ui.Appearance.SystemTheme")!;
            var methodInfo = type.GetMethod("GetTheme", BindingFlags.Static | BindingFlags.Public, Array.Empty<Type>())!;

            s_systemThemeGetter = methodInfo.CreateDelegate<Func<SystemThemeType>>();
        }

        private HwndSource? _hookHwndSource;
        private readonly IServiceProvider _serviceProvider;

        public ThemeService(
            IServiceProvider serviceProvider,
            ConfigService configService)
        {
            _serviceProvider = serviceProvider;
            ConfigService = configService;
        }

        public ConfigService ConfigService { get; }

        HwndSource CreateHookHwndSource()
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            var hwnd = new WindowInteropHelper(mainWindow).EnsureHandle();

            var hookHwndSource = HwndSource.FromHwnd(hwnd);
            hookHwndSource.AddHook(HwndHook);

            return hookHwndSource;
        }

        private nint HwndHook(nint hwnd, int msg, nint wParam, nint lParam, ref bool handled)
        {
            if (msg == 26)
                ApplyTheme();

            return nint.Zero;
        }

        [RelayCommand]
        public void ApplyTheme()
        {
            var appTheme = ConfigService.Config.Theme;
            var themeType = appTheme switch
            {
                AppTheme.Light => ThemeType.Light,
                AppTheme.Dark => ThemeType.Dark,
                _ => GetSystemTheme()
            };

            Theme.Apply(themeType, BackgroundType.Mica, true);

            if (appTheme == AppTheme.Auto && _hookHwndSource == null)
                _hookHwndSource = CreateHookHwndSource();
        }

        static ThemeType GetSystemTheme()
        {
            var systemTheme = s_systemThemeGetter.Invoke();

            ThemeType result = ThemeType.Light;
            if ((uint)(systemTheme - 3) < 2)
                result = ThemeType.Dark;

            return result;
        }
    }
}
