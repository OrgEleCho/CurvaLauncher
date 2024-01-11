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

    public PluginOption(CurvaLauncherPlugin plugin, string optionName, string? optionDescription, string optionPropertyName)
    {
        Plugin = plugin;
        OptionName = optionName;
        OptionDescription = optionDescription;
        OptionPropertyName = optionPropertyName;
    }

    public CurvaLauncherPlugin Plugin { get; }
    public string OptionPropertyName { get; }


    public string OptionName
    {
        get { return (string)GetValue(OptionNameProperty); }
        set { SetValue(OptionNameProperty, value); }
    }

    public string? OptionDescription
    {
        get { return (string)GetValue(OptionDescriptionProperty); }
        set { SetValue(OptionDescriptionProperty, value); }
    }

    public static readonly DependencyProperty OptionNameProperty =
        DependencyProperty.Register(nameof(OptionName), typeof(string), typeof(PluginOption), new PropertyMetadata("Option"));
    public static readonly DependencyProperty OptionDescriptionProperty =
        DependencyProperty.Register("OptionDescription", typeof(string), typeof(PluginOption), new PropertyMetadata(null));
}
