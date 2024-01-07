using CurvaLauncher.Plugin;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CurvaLauncher.Views.Components;

public partial class PluginTextOption : PluginOption
{
    public PluginTextOption(IPlugin plugin, string optionName, string optionPropertyName, bool allowMultiline) 
        : base(plugin, optionName, optionPropertyName)
    {
        InitializeComponent();

        input.AcceptsReturn = allowMultiline;
        input.SetBinding(TextBox.TextProperty, new Binding
        {
            Source = plugin,
            Path = new PropertyPath(optionPropertyName),
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        });
    }
}
