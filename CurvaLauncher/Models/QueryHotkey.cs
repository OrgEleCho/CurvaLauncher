using System;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Models;

public sealed partial class QueryHotkey : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsValidHotkey))]
    private string _hotkey = string.Empty;

    [ObservableProperty]
    private string _queryText = string.Empty;

    [JsonIgnore]
    public bool IsValidHotkey => HotkeyUtils.IsValidHotkey(Hotkey);
}
