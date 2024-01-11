using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CurvaLauncher.Apis;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace CurvaLauncher.PluginInteraction
{
    internal class CurvaLauncherContextImpl : CurvaLauncherContext
    {
        readonly ConfigService _configService;
        readonly I18nService _i18NService;

        private CurvaLauncherContextImpl()
        {
            _configService = App.ServiceProvider.GetRequiredService<ConfigService>();
            _i18NService = App.ServiceProvider.GetRequiredService<I18nService>();

            _i18NService.CurrentCultureChanged += I18NServiceCurrentCultureChanged;
        }

        private void I18NServiceCurrentCultureChanged(object? sender, EventArgs e)
        {
            AppLanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public static CurvaLauncherContextImpl Instance { get; } = new();

        public override Dispatcher Dispatcher => Application.Current.Dispatcher;

        public override int RequiredIconSize => _configService.Config.QueryResultIconSize;

        public override CultureInfo CurrentCulture => throw new NotImplementedException();

        public override ICommonApi Api => Apis.CommonApi.Instance;

        public override IFileApi FileApi => Apis.FileApi.Instance;
        public override IImageApi ImageApi => Apis.ImageApi.Instance;
        public override IStringApi StringApi => Apis.StringApi.Instance;
        public override ICommandLineApi CommandLineApi => Apis.CommandLineApi.Instance;


        public override event EventHandler? AppLanguageChanged;

        public override bool IsAltKeyPressed() => Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
        public override bool IsCtrlKeyPressed() => Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        public override bool IsShiftKeyPressed() => Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

        public override void AddI18nResourceDictionary(Assembly assembly, CultureInfo cultureInfo, ResourceDictionary resourceDictionary)
            => _i18NService.AddAssemblyResourceDictionary(assembly, cultureInfo, resourceDictionary);
        public override object GetI18nResourceValue(Assembly assembly, object? key) 
            => App.Current.Resources[new AssemblyResourceKey(assembly,key)];
        public override string? GetI18nResourceString(Assembly assembly, object? key) 
            => GetI18nResourceValue(assembly, key) as string;
    }
}
