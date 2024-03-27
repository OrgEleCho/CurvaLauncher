using System.Windows.Media;

namespace CurvaLauncher.Plugins.Everything
{
    public class EverythingQueryResult : ISyncQueryResult
    {
        private readonly EverythingPlugin _plugin;
        private readonly EverythingSearchClient.Result.Item _searchResult;
        private readonly string _itemFullPath;

        public EverythingQueryResult(EverythingPlugin plugin, EverythingSearchClient.Result.Item searchResult, float weight)
        {
            _plugin = plugin;
            _searchResult = searchResult;
            _itemFullPath = System.IO.Path.Combine(searchResult.Path, searchResult.Name);
            Weight = weight;

            plugin.HostContext.Dispatcher.Invoke(() =>
            {
                Icon = plugin.HostContext.ImageApi.GetFileIcon(_itemFullPath, plugin.HostContext.RequiredIconSize);

                if (Icon == null)
                {
                    if (System.IO.Directory.Exists(_itemFullPath))
                        Icon = plugin.HostContext.ImageApi.GetDefaultFolderIcon(plugin.HostContext.RequiredIconSize);
                    else if (System.IO.File.Exists(_itemFullPath))
                        Icon = plugin.HostContext.ImageApi.GetDefaultFileIcon(plugin.HostContext.RequiredIconSize);
                }
            });
        }

        public string Title => _searchResult.Name;
        public string Description => _searchResult.Path;

        public float Weight { get; }

        public ImageSource? Icon { get; private set; }

        public void Invoke()
        {
            if (_plugin.HostContext.IsAltKeyPressed())
            {
                _plugin.HostContext.Api.ShowPropertiesWindow(_itemFullPath);
            }
            else if (_plugin.HostContext.IsCtrlKeyPressed())
            {
                _plugin.HostContext.Api.ShowInFileExplorer(_itemFullPath);
            }
            else
            {
                _plugin.HostContext.Api.Open(_itemFullPath);
            }
        }
    }
}
