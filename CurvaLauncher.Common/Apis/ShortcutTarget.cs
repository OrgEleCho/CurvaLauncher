namespace CurvaLauncher.Apis
{
    public record class ShortcutTarget(string FileName, string? Arguments, string? WorkingDirectory, string? IconPath, int IconIndex, bool RequiresAdmin);
}
