using System.Windows.Media;

namespace CurvaLauncher.Plugin;

public abstract class CommandPlugin : IPlugin
{
    [PluginOption("Command prefix")]
    public string Prefix { get; set; } = ">";

    [PluginOption("Command name ignore cases")]
    public bool IgnoreCases { get; set; } = true;


    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract ImageSource Icon { get; }
    public abstract IEnumerable<string> CommandNames { get; }
}
