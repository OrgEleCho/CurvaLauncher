using System.Reflection;
using System.Windows;
using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugin;

public abstract class AsyncI18nPlugin : AsyncPlugin, II18nPlugin
{
    readonly Type pluginType;

    public override string Name => HostContext.GetI18nResourceString(pluginType.Assembly, NameKey) ?? GetType().Name;
    public override string Description => HostContext.GetI18nResourceString(pluginType.Assembly, DescriptionKey) ?? string.Empty;

    public abstract object NameKey { get; }
    public abstract object DescriptionKey { get; }

    protected AsyncI18nPlugin(CurvaLauncherContext context) : base(context)
    {
        pluginType = GetType();
        Assembly assembly = pluginType.Assembly;

        foreach (var i18nResourceDictionary in GetI18nResourceDictionaries())
            context.AddI18nResourceDictionary(assembly, i18nResourceDictionary.CultureInfo, i18nResourceDictionary.ResourceDictionary);

        context.AppLanguageChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
        };
    }

    public abstract IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries();
}