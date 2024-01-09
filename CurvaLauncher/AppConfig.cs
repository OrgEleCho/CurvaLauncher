using CommunityToolkit.Mvvm.ComponentModel;
using CurvaLauncher.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Wpf.Ui.Appearance;

namespace CurvaLauncher;

public partial class AppConfig : ObservableObject
{
    [ObservableProperty]
    private int _launcherWidth = 800;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(LauncherResultViewHeight))]
    private int _launcherResultViewCount = 7;

    [ObservableProperty]
    private int _queryResultIconSize = 64;

    [ObservableProperty]
    private bool _startsWithWindows = false;

    [ObservableProperty]
    private bool _keepLauncherWhenFocusLost = false;

    [ObservableProperty]
    private string _launcherHotkey = "Alt+Space";

    [ObservableProperty]
    private ObservableCollection<QueryHotkey> _customQueryHotkeys = new();

    [ObservableProperty]
    private Dictionary<string, PluginConfig> _plugins = new();

    [JsonIgnore]
    public double LauncherResultViewHeight => LauncherResultViewCount * 57 + LauncherResultViewCount;

    [ObservableProperty]
    private AppTheme _theme;

    [JsonIgnore]
    public IReadOnlyCollection<AppTheme> Themes { get; } = [AppTheme.Light, AppTheme.Dark];

    partial void OnThemeChanged(AppTheme value)
    {
        Wpf.Ui.Appearance.Theme.Apply(Theme switch
        {
            AppTheme.Dark => ThemeType.Dark,
            AppTheme.Light => ThemeType.Light,
            _ => ThemeType.Light,
        });
    }

    public partial class PluginConfig : ObservableObject
    {
        [ObservableProperty]
        private bool _isEnabled;

        [ObservableProperty]
        private float _weight;

        [ObservableProperty]
        JsonObject? _options;
    }
}
