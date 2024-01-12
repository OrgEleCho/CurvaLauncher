using System.Windows.Media;

namespace CurvaLauncher.Plugins;

public abstract class CommandPlugin : Plugin
{
    [PluginOption("Command prefix")]
    [PluginI18nOption("StrCommandPrefix")]
    public string Prefix { get; set; } = ">";

    [PluginOption("Command name ignore cases")]
    [PluginI18nOption("StrCommandIgnoreCases")]
    public bool IgnoreCases { get; set; } = true;

    protected CommandPlugin(CurvaLauncherContext context) : base(context)
    {

    }


    public abstract IEnumerable<string> CommandNames { get; }
}
