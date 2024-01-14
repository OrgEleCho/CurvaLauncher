using System.Windows.Media;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

namespace CurvaLauncher.Plugins.RunApplication;

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

        var sortedLogos = appInfo.AppLogos.OrderBy(logo => logo.Size);
        var sortedLargerLogos = sortedLogos.Where(logo => logo.Size >= context.RequiredIconSize);

        if (sortedLargerLogos.Any())
            Icon = context.Dispatcher.Invoke(() => new BitmapImage(new Uri(sortedLargerLogos.First().Path)));
        else if (sortedLogos.Any())
            Icon = context.Dispatcher.Invoke(() => new BitmapImage(new Uri(sortedLogos.First().Path)));
    }

    public void Invoke()
    {
        Process.Start(new ProcessStartInfo()
        {
            FileName = $@"shell:appsFolder\{AppInfo.FamilyID}!{AppInfo.ApplicationId}",
            UseShellExecute = true
        });
    }
}