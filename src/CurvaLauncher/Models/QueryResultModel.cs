using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CurvaLauncher.Messages;
using CurvaLauncher.Models.ImmediateResults;
using CurvaLauncher.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurvaLauncher.Models;

public partial class QueryResultModel : ObservableObject
{
    private readonly PluginInstance _pluginInstance;
    private readonly IQueryResult _rawQueryResult;

    public QueryResultModel(PluginInstance pluginInstance, IQueryResult rawQueryResult)
    {
        _pluginInstance = pluginInstance;
        _rawQueryResult = rawQueryResult;
        _icon = rawQueryResult.Icon;

        SetupFallbackIcon(() => pluginInstance.Plugin.Icon);
    }

    private ImageSource? _icon;

    public float Weight => _pluginInstance.Weight * _rawQueryResult.Weight;
    public string Title => _rawQueryResult.Title;
    public string Description => _rawQueryResult.Description;
    public ImageSource? Icon => _icon;

    private void SetupFallbackIcon(Func<ImageSource> iconFactory)
    {
        if (_icon == null)
        {
            SetProperty(ref _icon, iconFactory.Invoke(), nameof(Icon));
        }
        else if (_icon is BitmapImage bitmapImage && bitmapImage.IsDownloading)
        {
            var originIcon = _icon;

            SetProperty(ref _icon, iconFactory.Invoke(), nameof(Icon));
            bitmapImage.DownloadCompleted += (s, e) =>
            {
                SetProperty(ref _icon, originIcon, nameof(Icon));
            };
        }
    }

    public async Task<ImmediateResult?> Invoke(CancellationToken cancellationToken)
    {
        App.ServiceProvider
            .GetRequiredService<IMessenger>()
            .Send(SaveQueryMessage.Instance);

        try
        {
            if (_rawQueryResult is ISyncActionQueryResult syncQueryResult)
            {
                syncQueryResult.Invoke();
                App.CloseLauncher();
            }
            else if (_rawQueryResult is IAsyncActionQueryResult asyncQueryResult)
            {
                await asyncQueryResult.InvokeAsync(App.GetLauncherCancellationToken());
                App.CloseLauncher();
            }
            else if (_rawQueryResult is ISyncDocumentQueryResult syncDocumentQueryResult)
            {
                return new DocumentResult(syncDocumentQueryResult.GenerateDocument());
            }
            else if (_rawQueryResult is IAsyncDocumentQueryResult asyncDocumentQueryResult)
            {
                var document = await asyncDocumentQueryResult.GenerateDocumentAsync(cancellationToken);
                return new DocumentResult(document);
            }
            else if (_rawQueryResult is ISyncMenuQueryResult syncMenuQueryResult)
            {
                var menuItems = syncMenuQueryResult.GetMenuItems();
                return new MenuResult(_pluginInstance, menuItems.ToList());
            }
            else if (_rawQueryResult is IAsyncMenuQueryResult asyncMenuQueryResult)
            {
                var menuItems = await asyncMenuQueryResult.GetMenuItemsAsync(cancellationToken);
                return new MenuResult(_pluginInstance, menuItems.ToList());
            }
        }
        catch (OperationCanceledException)
        {
            // pass
        }
        catch (Exception ex)
        {
            MessageBox.Show($"{ex.Message}", "CurvaLauncher Result Invoke failed", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        return null;
    }
}
