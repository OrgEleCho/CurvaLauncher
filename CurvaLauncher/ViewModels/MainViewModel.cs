using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CurvaLauncher.Models;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly PluginService _pluginService;
    private readonly ConfigService _configService;

    public MainViewModel(
        PluginService pluginService,
        ConfigService configService)
    {
        _pluginService = pluginService;
        _configService = configService;
    }

    [ObservableProperty]
    private string queryText = string.Empty;

    [ObservableProperty]
    private ObservableCollection<QueryResultModel> queryResults = new();

    [ObservableProperty]
    private QueryResultModel? selectedQueryResult;

    [ObservableProperty]
    private int selectedQueryResultIndex = 0;

    public bool HasQueryResult => QueryResults.Count > 0;

    [RelayCommand]
    public async Task QueryCore(CancellationToken cancellationToken)
    {
        SelectedQueryResult = null;

        var dispatcher = Dispatcher.CurrentDispatcher;
        var queryText = QueryText;

        var context = new CurvaLauncherContext(dispatcher, _configService.Config.QueryResultIconSize);

        SortedCollection<QueryResultModel, float> queryResults = new()
        {
            SortingRoot = m => m.Weight,
            Descending = true,
        };

        await Task.Run(async () =>
        {
            foreach (var pluginInstance in _pluginService.PluginInstances)
            {
                await pluginInstance.InitTask;

                await foreach (var result in pluginInstance.QueryAsync(context, queryText))
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var model = QueryResultModel.FromQueryResult(result);
                    queryResults.Add(model);

                    dispatcher.Invoke(() =>
                    {
                        if (model.Icon == null)
                            model.Icon = pluginInstance.Plugin.Icon;

                        for (int i = 0; i < queryResults.Count; i++)
                        {
                            if (QueryResults.Count > i)
                                QueryResults[i] = queryResults[i];
                            else
                                QueryResults.Add(queryResults[i]);
                        }

                        if (SelectedQueryResult == null)
                            SelectedQueryResultIndex = 0;
                    });

                    if (cancellationToken.IsCancellationRequested)
                        return;
                }
            }
        });

        while (QueryResults.Count > queryResults.Count)
            QueryResults.RemoveAt(QueryResults.Count - 1);

        OnPropertyChanged(nameof(HasQueryResult));
    }

    [RelayCommand]
    public void Query()
    {
        if (QueryCoreCommand.IsRunning)
            QueryCoreCommand.Cancel();

        QueryCoreCommand.Execute(null);
    }

    [RelayCommand]
    public async Task InvokeSelected()
    {
        if (SelectedQueryResult == null)
            return;

        await SelectedQueryResult.InvokeCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    public void SelectNext()
    {
        SelectedQueryResultIndex = (SelectedQueryResultIndex + 1) % QueryResults.Count;
    }

    [RelayCommand]
    public void SelectPrev()
    {
        int newIndex = (SelectedQueryResultIndex - 1) % QueryResults.Count;
        if (newIndex == -1)
            newIndex = QueryResults.Count - 1;

        SelectedQueryResultIndex = newIndex;
    }
}
