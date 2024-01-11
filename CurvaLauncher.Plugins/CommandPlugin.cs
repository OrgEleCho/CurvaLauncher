using System.Windows.Media;

namespace CurvaLauncher.Plugins;

public abstract class CommandPlugin : Plugin
{
    [PluginOption("Command prefix")]
    public string Prefix { get; set; } = ">";

    [PluginOption("Command name ignore cases")]
    public bool IgnoreCases { get; set; } = true;

    protected CommandPlugin(CurvaLauncherContext context) : base(context)
    {
    }


    public abstract IEnumerable<string> CommandNames { get; }
}
