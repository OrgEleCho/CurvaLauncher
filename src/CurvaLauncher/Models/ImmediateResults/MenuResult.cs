using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CurvaLauncher.Models.ImmediateResults
{
    public partial class MenuResult : ImmediateResult
    {
        [ObservableProperty]
        private int _selectedIndex;

        [ObservableProperty]
        private QueryResultModel? _selectedItem;

        public IReadOnlyList<QueryResultModel> Items { get; }

        public MenuResult(PluginInstance pluginInstance, IReadOnlyList<IQueryResult> items)
        {
            Items = items
                .Select(item => new QueryResultModel(pluginInstance, item))
                .OrderByDescending(item => item.Weight)
                .ToArray();
        }
    }
}
