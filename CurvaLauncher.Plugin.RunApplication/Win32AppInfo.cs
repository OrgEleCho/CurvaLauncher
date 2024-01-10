namespace CurvaLauncher.Plugin.RunApplication;

public class Win32AppInfo : AppInfo
{
    public string? CommandArguments { get; set; }
    public string? WorkingDirectory { get; set; }
    public string? IconPath { get; set; }
    public bool IsUAC { get; set; } = false;
}
