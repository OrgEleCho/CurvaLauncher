namespace CurvaLauncher.Plugin;

[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class PluginOptionAttribute : Attribute
{
    public string? Name { get; set; }
    public string? Description { get; set; }

    public PluginOptionAttribute() { }

    public PluginOptionAttribute(string? name)
    {
        Name = name;
    }

    public PluginOptionAttribute(string? name, string? description)
    {
        Name = name;
        Description = description;
    }
}