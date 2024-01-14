namespace CurvaLauncher.Plugins;

public interface II18nPlugin : IPlugin
{
    public object NameKey { get; }
    public object DescriptionKey { get; }
}