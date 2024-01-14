using CurvaLauncher.Plugins;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CurvaLauncher.Views.Components;

public partial class PluginTextOption : PluginOption
{
    public PluginTextOption(
        IPlugin plugin,
        string optionName,
        string? optionDescription,
        string optionPropertyName,
        bool allowMultiline) 
        : base(plugin, optionName, optionDescription, optionPropertyName)
    {
        InitializeComponent();
        BuildOptionControl(allowMultiline);
    }

    public PluginTextOption(
        Assembly resourceAssembly,
        IPlugin plugin,
        object optionNameKey,
        object? optionDescriptionKey,
        string optionPropertyName,
        bool allowMultiline) : base(resourceAssembly, plugin, optionNameKey, optionDescriptionKey, optionPropertyName)
    {
        InitializeComponent();
        BuildOptionControl(allowMultiline);
    }

    void BuildOptionControl(bool allowMultiline)
    {
        input.AcceptsReturn = allowMultiline;
        input.SetBinding(TextBox.TextProperty, new Binding
        {
            Source = Plugin,
            Path = new PropertyPath(OptionPropertyName),
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        });
    }
}
