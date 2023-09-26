using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MicaLauncher.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MicaLauncher.Models
{
    public partial class QueryResultModel : ObservableObject
    {
        private readonly Action _invokeAction;

        public QueryResultModel(float match, string title, string description, ImageSource? icon, Action invokeAction)
        {
            Match = match;
            Title = title;
            Description = description;
            Icon = icon;
            _invokeAction = invokeAction;
        }

        [ObservableProperty]
        private float match;

        [ObservableProperty]
        private string title = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private ImageSource? icon;

        [RelayCommand]
        public void Invoke()
        {
            _invokeAction.Invoke();

            MainWindow mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();

            App.CloseLauncher();
        }

        public static QueryResultModel FromQueryResult(QueryResult queryResult)
        {
            return new QueryResultModel(queryResult.Match, queryResult.Title, queryResult.Description, queryResult.Icon, queryResult.Invoke);
        }
    }
}
