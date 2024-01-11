using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;

namespace CurvaLauncher.Services
{
    public partial class I18nService
    {
        private readonly ConfigService _configService;

        private ResourceDictionary? _loadedDictionary;
        readonly Dictionary<CultureInfo, ResourceDictionary> _resourceDictionaries = new()
        {
            [new CultureInfo("en-US")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("EnUs.xaml") },
            [new CultureInfo("zh-Hans")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("ZhHans.xaml") },
            [new CultureInfo("zh-Hant")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("ZhHant.xaml") },
            [new CultureInfo("ja-JP")] = new ResourceDictionary() { Source = GetResourceDictionaryPath("JaJp.xaml") },
        };

        public I18nService(
            ConfigService configService)
        {
            _configService = configService;
        }

        private ResourceDictionary? GetMatchedResourceDictionary(CultureInfo cultureInfo)
        {
            foreach (var resourceKV in _resourceDictionaries)
            {
                if (cultureInfo.Equals(resourceKV.Key))
                    return resourceKV.Value;
            }

            foreach (var resourceKV in _resourceDictionaries)
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
            return new Uri($"pack://application:,,,/Globalization/{filename}");
        }

        [RelayCommand]
        public void ApplyLanguage()
        {
            var appLanguage = _configService.Config.Language;
            var cultureInfo = appLanguage switch
            {
                AppLanguage.English => new CultureInfo("en"),
                AppLanguage.ChineseSimplified => new CultureInfo("zh-Hans"),
                AppLanguage.ChineseTraditional => new CultureInfo("zh-Hant"),
                AppLanguage.Japanese => new CultureInfo("ja-JP"),
                _ => CultureInfo.CurrentCulture
            };

            var resourceDictionary = GetMatchedResourceDictionary(cultureInfo);

            if (resourceDictionary != null)
                ApplyResourceDictionary(resourceDictionary);
        }
    }
}
