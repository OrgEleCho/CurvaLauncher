using System.Windows.Media;

namespace CurvaLauncher.Plugin;

public abstract class CurvaLauncherCommandPlugin : CurvaLauncherPlugin
{
    [PluginOption("Command prefix")]
    public string Prefix { get; set; } = ">";

    [PluginOption("Command name ignore cases")]
    public bool IgnoreCases { get; set; } = true;

    protected CurvaLauncherCommandPlugin(CurvaLauncherContext context) : base(context)
    {
    }


    public abstract IEnumerable<string> CommandNames { get; }
}
