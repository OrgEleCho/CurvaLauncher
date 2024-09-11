using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using CurvaLauncher;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace CurvaLauncher.Plugins.RunApplication;

public class RunWin32ApplicationQueryResult : IAsyncQueryResult
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
        Weight = weight;

        context.Dispatcher.Invoke(() =>
        {
            var iconPath = appInfo.IconPath ?? appInfo.FilePath;

            try
            {
                if (!iconPath.EndsWith(".ico"))
                {
                    var iconIndex = appInfo.IconIndex;
                    icon = context.ImageApi.GetEmbededIconImage(iconPath, context.RequiredIconSize, iconIndex);
                }
                else
                {
                    if (File.Exists(iconPath))
                        icon = new BitmapImage(new Uri(iconPath));
                }
            }
            catch { }
        });
    }

    public async Task InvokeAsync(CancellationToken cancellationToken)
    {
        try
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

            if (process is null)
            {
                return;
            }

            try
            {
                await Task.Run(process.WaitForInputIdle);
            }
            catch (InvalidOperationException)
            {
                // can not wait process, ignore
            }
        }
        catch (Exception)
        {
            // other exception, ignore
        }
    }
}
