namespace CurvaLauncher.Plugin;

public interface ICurvaLauncherI18nPlugin : ICurvaLauncherPlugin
{
    public object NameKey { get; }
    public object DescriptionKey { get; }
}