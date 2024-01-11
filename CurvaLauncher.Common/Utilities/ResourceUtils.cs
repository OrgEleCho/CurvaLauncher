using System.Windows;

namespace CurvaLauncher.Apis;

public static class ResourceUtils
{
    public static string? GetString(this ResourceDictionary resourceDictionary, object key)
    {
        if (resourceDictionary.Contains(key) && resourceDictionary[key] is string value)
            return value;

        return null;
    }
}
