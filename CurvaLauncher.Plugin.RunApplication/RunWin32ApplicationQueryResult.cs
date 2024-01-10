using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using CurvaLauncher;
using CurvaLauncher.Utilities;
using System.Windows.Media.Imaging;
using System.Text.RegularExpressions;

namespace CurvaLauncher.Plugin.RunApplication;

public class RunWin32ApplicationQueryResult : ISyncQueryResult
{

    private ImageSource? icon;

    public float Weight { get; }

    public string Title => $"{AppInfo.Name}";

    public string Description => $"Run Application: {AppInfo.Name}";

    public ImageSource? Icon => icon;

    public Win32AppInfo AppInfo { get; }


    public RunWin32ApplicationQueryResult(CurvaLauncherContext context, Win32AppInfo appInfo, float weight)
    {
        AppInfo = appInfo;

        context.Dispatcher.Invoke(() =>
        {
            var iconPath = appInfo.IconPath ?? appInfo.FilePath;
            var iconIndex = appInfo.IconIndex;

            icon = ImageUtils.GetEmbededIconImage(iconPath, context.RequiredIconSize, iconIndex);
        });
    }

    public void Invoke()
    {
        var process = Process.Start(
            new ProcessStartInfo()
            {
                FileName = AppInfo.FilePath,
                Arguments = AppInfo.Arguments,
                WorkingDirectory = AppInfo.WorkingDirectory ?? Path.GetDirectoryName(AppInfo.FilePath),
                Verb = AppInfo.IsUAC ? "runas" : null,
                UseShellExecute = true,
            });
    }
}

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