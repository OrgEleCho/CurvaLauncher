
namespace CurvaLauncher.Plugins.RunApplication;

public record class Win32AppInfo : AppInfo, IEquatable<Win32AppInfo?>
{
    public string FilePath { get; set; } = string.Empty;
    public string? Arguments { get; set; }
    public string? WorkingDirectory { get; set; }
    public string? IconPath { get; set; }
    public int IconIndex { get; set; }
    public bool IsUAC { get; set; }

    public required string OriginShortcutPath { get; set; } = string.Empty;

    public override int GetHashCode()
        => HashCode.Combine(Name, FilePath, Arguments, WorkingDirectory, IconPath, IconIndex, IsUAC, OriginShortcutPath);

    public void Populate(Win32AppInfo newInfo)
    {
        Name = newInfo.Name;
        FilePath = newInfo.FilePath;
        Arguments = newInfo.Arguments;
        WorkingDirectory = newInfo.WorkingDirectory;
        IconPath = newInfo.IconPath;
        IconIndex = newInfo.IconIndex;
        IsUAC = newInfo.IsUAC;
        OriginShortcutPath = newInfo.OriginShortcutPath;
    }

    public bool IsSame(Win32AppInfo another, Win32AppDistinctMode mode)
    {
        return mode switch
        {
            Win32AppDistinctMode.FilePath
                => FilePath == another.FilePath,
            Win32AppDistinctMode.FilePathAndArguments
                => FilePath == another.FilePath && Arguments == another.Arguments,
            _ => false,
        };
    }

}
