using CurvaLauncher.Plugin;
using System.Windows;
using System.Windows.Controls;

namespace CurvaLauncher.Views.Components;

public class PluginOption : UserControl
{
    static PluginOption()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PluginOption), new FrameworkPropertyMetadata(typeof(PluginOption)));
    }

    public PluginOption(IPlugin plugin, string optionName, string optionPropertyName)
    {
        Plugin = plugin;
        OptionName = optionName;
        OptionPropertyName = optionPropertyName;
    }


    public string OptionName
    {
        get { return (string)GetValue(OptionNameProperty); }
        set { SetValue(OptionNameProperty, value); }
    }

    public IPlugin Plugin { get; }
    public string OptionPropertyName { get; }

    public static readonly DependencyProperty OptionNameProperty =
        DependencyProperty.Register(nameof(OptionName), typeof(string), typeof(PluginOption), new PropertyMetadata("Option"));


}
