using CurvaLauncher.Plugin;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace CurvaLauncher.Views.Components;

public partial class PluginOptionsControl : UserControl
{
    public IPlugin Plugin { get; }

    public PluginOptionsControl(IPlugin plugin)
    {
        Plugin = plugin;

        InitializeComponent();
        BuildOptions();
    }

    private PluginOption? CreateOption(PluginOptionAttribute attribute, PropertyInfo property)
    {
        if (typeof(bool) == property.PropertyType)
        {
            return new PluginSwitchOption(Plugin, attribute.Name ?? property.Name, property.Name);
        }
        else if (property.PropertyType.IsEnum)
        {
            if (property.PropertyType.GetCustomAttribute<FlagsAttribute>() is FlagsAttribute)
            {
                return new PluginFlagsOption(Plugin, attribute.Name ?? property.Name, property, property.PropertyType);
            }
            else
            {
                return new PluginSelectOption(Plugin, attribute.Name ?? property.Name, property.Name, Enum.GetValues(property.PropertyType));
            }
        }
        else if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
        {
            return new PluginTextOption(Plugin, attribute.Name ?? property.Name, property.Name, attribute.AllowTextMultiline);
        }
        else
        {
            return null;
        }
    }


    private void BuildOptions()
    {
        var props = Plugin.GetType().GetProperties()
            .Select(p => (Attribute: p.GetCustomAttribute<PluginOptionAttribute>(), Property: p))
            .Where(v => v.Attribute is not null);

        foreach (var item in props)
        {
            if (CreateOption(item.Attribute!, item.Property) is PluginOption pluginOption)
            {
                pluginOption.Margin = new System.Windows.Thickness(0, 0, 0, 15);
                optionsPanel.Children.Add(pluginOption);
            }
        }
        
    }
}
