using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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
    private JsonObject? _pluginsConfig;

    [JsonIgnore]
    public double LauncherResultViewHeight => LauncherResultViewCount * 50;
}
