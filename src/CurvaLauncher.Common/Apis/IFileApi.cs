namespace CurvaLauncher.Apis;

public interface IFileApi
{
    public ShortcutTarget? GetShortcutTarget(string file);
}
