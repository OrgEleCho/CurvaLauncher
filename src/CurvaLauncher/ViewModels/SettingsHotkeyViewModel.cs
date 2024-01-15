using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CurvaLauncher.ViewModels;

public partial class SettingsHotkeyViewModel : ObservableObject
{
    public ObservableCollection<string> ErrorHotkeys { get; } = new();
    public bool HasErrorHotkey => ErrorHotkeys.Any();

    public SettingsHotkeyViewModel()
    {
        ErrorHotkeys.CollectionChanged += (s, e) =>
        {
            OnPropertyChanged(nameof(HasErrorHotkey));
        };
    }
}
