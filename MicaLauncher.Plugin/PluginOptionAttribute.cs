namespace MicaLauncher.Plugin
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class PluginOptionAttribute : Attribute
    {
        public string? Description { get; set; }
    }
}