using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using CurvaLauncher.Utilities.Resources;
using Microsoft.Extensions.DependencyInjection;

namespace CurvaLauncher.Services
{
    public partial class I18nService
    {
        record class I18nResourceDictionaries(ResourceDictionary Default, Dictionary<CultureInfo, ResourceDictionary> All);

        private readonly IServiceProvider _serviceProvider;
        private readonly ConfigService _configService;

        private ResourceDictionary? _loadedDictionary;
        readonly Dictionary<CultureInfo, ResourceDictionary> _resourceDictionaries = new()
        {
            [new CultureInfo("en-US")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("EnUs.xaml") },
            [new CultureInfo("zh-Hans")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("ZhHans.xaml") },
            [new CultureInfo("zh-Hant")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("ZhHant.xaml") },
            [new CultureInfo("ja-JP")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("JaJp.xaml") },
        };

        readonly Dictionary<Assembly, I18nResourceDictionaries> _assemblyResourceDictionaries = new();

        public I18nService(
            IServiceProvider serviceProvider,
            ConfigService configService)
        {
            _serviceProvider = serviceProvider;
            _configService = configService;
        }

        private ResourceDictionary? GetMatchedResourceDictionary(CultureInfo cultureInfo, Dictionary<CultureInfo, ResourceDictionary> resourceDictionaries)
        {
            foreach (var resourceKV in resourceDictionaries)
            {
                if (cultureInfo.Equals(resourceKV.Key))
                    return resourceKV.Value;
            }

            foreach (var resourceKV in resourceDictionaries)
            {
                if (cultureInfo.TwoLetterISOLanguageName.Equals(resourceKV.Key.TwoLetterISOLanguageName))
                    return resourceKV.Value;
            }

            return null;
        }

        private void ApplyResourceDictionary(ResourceDictionary resourceDictionary)
        {
            if (_loadedDictionary != null)
                App.Current.Resources.MergedDictionaries.Remove(_loadedDictionary);

            App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
            _loadedDictionary = resourceDictionary;
        }

        private static Uri GetResourceDictionaryPath(string filename)
        {
            return new Uri($"pack://application:,,,/I18n/{filename}");
        }

        public CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentCulture;

        [RelayCommand]
        public void ApplyLanguage()
        {
            var appLanguage = _configService.Config.Language;
            var pluginService = _serviceProvider.GetRequiredService<PluginService>();

            CurrentCulture = appLanguage switch
            {
                AppLanguage.English => new CultureInfo("en"),
                AppLanguage.ChineseSimplified => new CultureInfo("zh-Hans"),
                AppLanguage.ChineseTraditional => new CultureInfo("zh-Hant"),
                AppLanguage.Japanese => new CultureInfo("ja-JP"),
                _ => CultureInfo.CurrentCulture
            };

            ResourceDictionary i18nDictionary = new();

            if (GetMatchedResourceDictionary(CurrentCulture, _resourceDictionaries) is ResourceDictionary resourceDictionary)
                i18nDictionary.MergedDictionaries.Add(resourceDictionary);

            foreach (var assemblyResourceDictionariesKV in _assemblyResourceDictionaries)
            {
                if (GetMatchedResourceDictionary(CurrentCulture, assemblyResourceDictionariesKV.Value.All) is ResourceDictionary assemblyResourceDictionary)
                    i18nDictionary.MergedDictionaries.Add(new AssemblyResourceDictionary(assemblyResourceDictionariesKV.Key, assemblyResourceDictionary));
                else
                    i18nDictionary.MergedDictionaries.Add(new AssemblyResourceDictionary(assemblyResourceDictionariesKV.Key, assemblyResourceDictionariesKV.Value.Default));
            }

            ApplyResourceDictionary(i18nDictionary);

            OnCurrentCultureChanged();
        }

        public void AddAssemblyResourceDictionary(Assembly assembly, CultureInfo cultureInfo, ResourceDictionary resourceDictionary)
        {
            if (!_assemblyResourceDictionaries.TryGetValue(assembly, out var dicts))
                _assemblyResourceDictionaries[assembly] = dicts = new(resourceDictionary, new());

            dicts.All[cultureInfo] = resourceDictionary;
        }

        private void OnCurrentCultureChanged()
            => CurrentCultureChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler? CurrentCultureChanged;
    }
}
