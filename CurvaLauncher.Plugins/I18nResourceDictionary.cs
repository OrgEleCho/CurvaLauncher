using System.Globalization;
using System.Reflection;
using System.Windows;

namespace CurvaLauncher.Plugins;

public record class I18nResourceDictionary(Assembly Assembly, CultureInfo CultureInfo, ResourceDictionary ResourceDictionary)
{
    public static I18nResourceDictionary Create(CultureInfo cultureInfo, string resourcePath)
    {
        resourcePath = resourcePath.TrimStart('/', '\\');

        Assembly callingAssembly = Assembly.GetCallingAssembly();

        return new I18nResourceDictionary(callingAssembly, cultureInfo,
            new ResourceDictionary() 
            { 
                Source = new Uri(new Uri($"pack://application:,,,/{callingAssembly.FullName};component/"), resourcePath)
            });
    }
}
