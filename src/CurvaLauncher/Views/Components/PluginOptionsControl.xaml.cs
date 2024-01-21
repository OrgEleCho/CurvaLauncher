using CurvaLauncher.Models;
using CurvaLauncher.Plugins;
using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Data;

namespace CurvaLauncher.Views.Components;

public partial class PluginOptionsControl : UserControl
{
    public CurvaLauncherPluginInstance PluginInstance { get; }

    public PluginOptionsControl(CurvaLauncherPluginInstance pluginInstance)
    {
        PluginInstance = pluginInstance;

        InitializeComponent();
        BuildOptions();
    }

    private PluginOption? CreateOption(PluginOptionAttribute attribute, PropertyInfo property)
    {
        if (typeof(bool) == property.PropertyType)
        {
            return new PluginSwitchOption(PluginInstance.Plugin, attribute.Name ?? property.Name, attribute.Description, property.Name);
        }
        else if (property.PropertyType.IsEnum)
        {
            if (property.PropertyType.GetCustomAttribute<FlagsAttribute>() is FlagsAttribute)
            {
                return new PluginFlagsOption(PluginInstance.Plugin, attribute.Name ?? property.Name, attribute.Description, property, property.PropertyType);
            }
            else
            {
                return new PluginSelectOption(PluginInstance.Plugin, attribute.Name ?? property.Name, attribute.Description, property.Name, Enum.GetValues(property.PropertyType));
            }
        }
        else if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
        {
            return new PluginTextOption(PluginInstance.Plugin, attribute.Name ?? property.Name, attribute.Description, property.Name, attribute.AllowTextMultiline);
        }
        else
        {
            return null;
        }
    }

    private PluginOption? CreateI18nOption(PluginI18nOptionAttribute attribute, PropertyInfo property)
    {
        Assembly resourceAssembly = property.DeclaringType!.Assembly;

        if (typeof(bool) == property.PropertyType)
        {
            return new PluginSwitchOption(resourceAssembly, PluginInstance.Plugin, attribute.NameKey, attribute.DescriptionKey, property.Name);
        }
        else if (property.PropertyType.IsEnum)
        {
            if (property.PropertyType.GetCustomAttribute<FlagsAttribute>() is FlagsAttribute)
            {
                return new PluginFlagsOption(resourceAssembly, PluginInstance.Plugin, attribute.NameKey, attribute.DescriptionKey, property, property.PropertyType);
            }
            else
            {
                return new PluginSelectOption(resourceAssembly, PluginInstance.Plugin, attribute.NameKey, attribute.DescriptionKey, property.Name, Enum.GetValues(property.PropertyType));
            }
        }
        else if (typeof(IConvertible).IsAssignableFrom(property.PropertyType))
        {
            return new PluginTextOption(resourceAssembly, PluginInstance.Plugin, attribute.NameKey, attribute.DescriptionKey, property.Name, attribute.AllowTextMultiline);
        }
        else
        {
            return null;
        }
    }

    private void BuildOptions()
    {
        foreach (var prop in PluginInstance.Plugin.GetType().GetProperties())
        {
            if (PluginInstance.Plugin is II18nPlugin && 
                prop.GetCustomAttribute<PluginI18nOptionAttribute>() is { } i18nAttr)
            {
                if (CreateI18nOption(i18nAttr, prop) is PluginOption pluginOption)
                {
                    pluginOption.Margin = new System.Windows.Thickness(0, 0, 0, 20);
                    optionsPanel.Children.Add(pluginOption);
                }
            }
            else if (prop.GetCustomAttribute<PluginOptionAttribute>() is { } attr)
            {
                if (CreateOption(attr, prop) is PluginOption pluginOption)
                {
                    pluginOption.Margin = new System.Windows.Thickness(0, 0, 0, 20);
                    optionsPanel.Children.Add(pluginOption);
                }
            }
        }
    }
}
