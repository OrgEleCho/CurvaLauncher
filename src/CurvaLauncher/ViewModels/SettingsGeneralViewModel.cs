using CommunityToolkit.Mvvm.ComponentModel;

namespace CurvaLauncher.ViewModels;

public partial class SettingsGeneralViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _hotkeyNotRegistered;

    [ObservableProperty]
    private bool _hotkeyNotValid;

    [ObservableProperty]
    private bool _showHotkeyApplyButton;
}
