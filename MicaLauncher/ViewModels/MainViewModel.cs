using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicaLauncher.Common;
using MicaLauncher.Data;
using MicaLauncher.Models;
using MicaLauncher.Services;

namespace MicaLauncher.ViewModels
{
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
            QueryResults.Clear();
            SelectedQueryResult = null;

            var dispatcher = Dispatcher.CurrentDispatcher;
            var queryText = QueryText;

            var context = new MicaLauncherContext(dispatcher, _configService.Config.QueryResultIconSize);

            await Task.Run(() =>
            {
                foreach (var plugin in _pluginService.Plugins)
                {
                    foreach (var result in plugin.Query(context, queryText))
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return;

                        var model = QueryResultModel.FromQueryResult(result);

                        if (model.Icon == null)
                            model.Icon = plugin.Icon;

                        dispatcher.Invoke(() =>
                        {
                            QueryResults.Add(model);

                            if (SelectedQueryResult == null)
                                SelectedQueryResultIndex = 0;
                        });
                    }

                    if (cancellationToken.IsCancellationRequested)
                        return;
                }
            });

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
