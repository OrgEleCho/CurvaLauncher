using System.Globalization;
using System.Reflection;
using System.Windows;

namespace CurvaLauncher.Plugin;

public record class I18nResourceDictionary(CultureInfo CultureInfo, ResourceDictionary ResourceDictionary)
{
    public static I18nResourceDictionary Create(CultureInfo cultureInfo, string resourcePath)
    {
        Assembly callingAssembly = Assembly.GetCallingAssembly();

        return new I18nResourceDictionary(cultureInfo,
            new ResourceDictionary() 
            { 
                Source = new Uri(new Uri($"pack://application:,,,/{callingAssembly.FullName};component/"), resourcePath)
            });
    }
}
