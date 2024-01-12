using System.Globalization;
using System.Reflection;

namespace CurvaLauncher.Plugins;

public abstract class CommandSyncI18nPlugin : CommandSyncPlugin, II18nPlugin
{
    readonly Type _pluginType;

    public override string Name => HostContext.GetI18nResourceString(_pluginType.Assembly, NameKey) ?? GetType().Name;
    public override string Description => HostContext.GetI18nResourceString(_pluginType.Assembly, DescriptionKey) ?? string.Empty;

    public abstract object NameKey { get; }
    public abstract object DescriptionKey { get; }

    protected CommandSyncI18nPlugin(CurvaLauncherContext context) : base(context)
    {
        _pluginType = GetType();

        foreach (var i18nResourceDictionary in GetI18nResourceDictionaries())
            context.AddI18nResourceDictionary(i18nResourceDictionary.Assembly, i18nResourceDictionary.CultureInfo, i18nResourceDictionary.ResourceDictionary);

        context.AppLanguageChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Description));
        };
    }

    public virtual IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
    {
        yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hant"), "I18n/ZhHant.xaml");
        yield return I18nResourceDictionary.Create(new CultureInfo("ja-JP"), "I18n/JaJp.xaml");
    }
}
