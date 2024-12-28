using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CurvaLauncher.Models.ImmediateResults
{
    public partial class MenuResult : ImmediateResult
    {
        [ObservableProperty]
        private int _selectedIndex;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PreviewPaneWidth))]
        private QueryResultModel? _selectedItem;

        public GridLength PreviewPaneWidth
        {
            get
            {
                if (SelectedItem is null ||
                    !SelectedItem.HasPreview)
                {
                    return new GridLength(0);
                }

                return new GridLength(2, GridUnitType.Star);
            }
        }

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
