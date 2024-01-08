using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CurvaLauncher.Models;

public partial class QueryResultModel : ObservableObject
{
    private readonly QueryResult _rawQueryResult;

    public QueryResultModel(float weight, string title, string description, ImageSource? icon, QueryResult rawQueryResult)
    {
        Weight = weight;
        Title = title;
        Description = description;
        Icon = icon;
        _rawQueryResult = rawQueryResult;
    }

    [ObservableProperty]
    private float weight;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private ImageSource? icon;

    public void SetFallbackIcon(Func<ImageSource> iconFactory)
    {
        if (Icon == null)
        {
            Icon = iconFactory.Invoke();
        }
        else if (Icon is BitmapImage bitmapImage && bitmapImage.IsDownloading)
        {
            var originIcon = Icon;

            Icon = iconFactory.Invoke();
            bitmapImage.DownloadCompleted += (s, e) =>
            {
                Icon = originIcon;
            };
        }
    }

    [RelayCommand]
    public async Task Invoke()
    {
        if (_rawQueryResult is SyncQueryResult syncQueryResult)
        {
            try
            {
                syncQueryResult.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "CurvaLauncher Result Invoke failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        else if (_rawQueryResult is AsyncQueryResult asyncQueryResult)
        {
            try
            {
                await asyncQueryResult.InvokeAsync(App.GetLauncherCancellationToken());
            }
            catch (OperationCanceledException)
            {
                // pass
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "CurvaLauncher Result Invoke failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //MainWindow mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
        App.CloseLauncher();
    }

    public static QueryResultModel FromQueryResult(QueryResult queryResult)
    {
        return new QueryResultModel(queryResult.Weight, queryResult.Title, queryResult.Description, queryResult.Icon, queryResult);
    }
}
