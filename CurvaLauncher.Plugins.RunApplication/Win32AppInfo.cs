namespace CurvaLauncher.Plugins.RunApplication;

public class Win32AppInfo : AppInfo
{
    public string FilePath { get; set; } = string.Empty;
    public string? Arguments { get; set; }
    public string? WorkingDirectory { get; set; }
    public string? IconPath { get; set; }
    public int IconIndex { get; set; }
    public bool IsUAC { get; set; }
}
