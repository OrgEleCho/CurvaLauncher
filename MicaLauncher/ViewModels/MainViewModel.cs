using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicaLauncher.Data;
using MicaLauncher.Models;
using MicaLauncher.Services;

namespace MicaLauncher.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly PluginService _pluginService;

        public MainViewModel(
            PluginService pluginService)
        {
            _pluginService = pluginService;
        }

        [ObservableProperty]
        private string queryText = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasQueryResult))]
        private ObservableCollection<QueryResultModel> queryResults = new();

        [ObservableProperty]
        private QueryResultModel? selectedQueryResult;

        [ObservableProperty]
        private int selectedQueryResultIndex = 0;

        public bool HasQueryResult => QueryResults.Count > 0;

        [RelayCommand]
        public async Task QueryCore(CancellationToken cancellationToken)
        {
            QueryResults.Clear();

            var dispatcher = Dispatcher.CurrentDispatcher;
            var queryText = QueryText;

            await Task.Run(() =>
            {
                foreach (var plugin in _pluginService.Plugins)
                {
                    foreach (var result in plugin.Query(dispatcher, queryText))
                    {
                        var model = QueryResultModel.FromQueryResult(result);

                        if (model.Icon == null)
                            model.Icon = plugin.Icon;

                        dispatcher.Invoke(() =>
                        {
                            QueryResults.Add(model);

                            if (SelectedQueryResult == null)
                                SelectedQueryResult = QueryResults[0];
                        });

                        if (cancellationToken.IsCancellationRequested)
                            return;
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                }
            });
        }

        [RelayCommand]
        public void Query()
        {
            if (QueryCoreCommand.IsRunning)
                QueryCoreCommand.Cancel();

            QueryCoreCommand.Execute(null);
        }

        [RelayCommand]
        public void InvokeSelected()
        {
            if (SelectedQueryResult == null)
                return;

            SelectedQueryResult.Invoke();
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
}
