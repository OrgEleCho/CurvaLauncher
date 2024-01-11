using System.Reflection;
using System.Windows;
using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugin;

public abstract class SyncI18nPlugin : SyncPlugin, II18nPlugin
{
    readonly Type _pluginType;

    public override string Name => HostContext.GetI18nResourceString(_pluginType.Assembly, NameKey) ?? GetType().Name;
    public override string Description => HostContext.GetI18nResourceString(_pluginType.Assembly, DescriptionKey) ?? string.Empty;

    public abstract object NameKey { get; }
    public abstract object DescriptionKey { get; }

    protected SyncI18nPlugin(CurvaLauncherContext context) : base(context)
    {
        _pluginType = GetType();
        Assembly assembly = _pluginType.Assembly;

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
