using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Nodes;

namespace CurvaLauncher;

public partial class AppConfig : ObservableObject
{
    [ObservableProperty]
    private int _launcherWidth = 800;

    [ObservableProperty]
    private int _queryResultIconSize = 64;

    [ObservableProperty]
    private bool _keepLauncherWhenFocusLost = false;

    [ObservableProperty]
    private string _launcherHotkey = "Alt+Space";

    [ObservableProperty]
    private JsonObject? _pluginsConfig;
}
