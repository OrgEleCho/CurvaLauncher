using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace CurvaLauncher.Plugin.RunApplication;

public class RunUwpApplicationQueryResult : ISyncQueryResult
{
    public UwpAppInfo AppInfo { get; }

    public string Title => $"{AppInfo.Name}";

    public string Description => $"Run Application: {AppInfo.Name}";

    public float Weight { get; }

    public ImageSource? Icon { get; private set; }

    public RunUwpApplicationQueryResult(CurvaLauncherContext context, UwpAppInfo appInfo, float weight)
    {
        AppInfo = appInfo;
        Weight = weight;

        if (File.Exists(appInfo.LogoPath) && 
            Uri.TryCreate(appInfo.LogoPath, UriKind.Absolute, out var logoUri))
        {
            context.Dispatcher.Invoke(() =>
            {
                Icon = new BitmapImage(logoUri);
            });
        }
    }

    public void Invoke()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = $@"shell:appsFolder\Microsoft.MinecraftUWP_8wekyb3d8bbwe!{AppInfo.ApplicationId}",
            UseShellExecute = true
        });
    }
}