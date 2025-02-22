namespace CurvaLauncher.Plugins;

[AttributeUsage(AttributeTargets.Assembly)]
public sealed class PluginTypeAttribute : Attribute
{
    public Type PluginType { get; private set; }

    public PluginTypeAttribute(Type pluginType)
    {
        PluginType = pluginType;
    }
}
