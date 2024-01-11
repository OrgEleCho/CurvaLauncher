using System.IO;

namespace CurvaLauncher.Plugins.RunApplication;

public class UwpAppInfo : AppInfo
{
    public string PackageID { get; set; } = string.Empty;
    public string PackageRootFolder { get; set; } = string.Empty;
    public string AppxManifestPath => Path.Combine(PackageRootFolder, "AppxManifest.xml");

    public string ApplicationId { get; set; } = string.Empty;
    public string LogoPath { get; set; } = string.Empty;
}