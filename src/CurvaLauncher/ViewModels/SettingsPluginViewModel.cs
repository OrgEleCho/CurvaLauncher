using CommunityToolkit.Mvvm.ComponentModel;
using CurvaLauncher.Models;

namespace CurvaLauncher.ViewModels;

public partial class SettingsPluginViewModel : ObservableObject
{
    [ObservableProperty]
    private PluginInstance? _selectedPluginInstance;
}
