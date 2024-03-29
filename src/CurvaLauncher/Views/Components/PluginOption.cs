﻿using CurvaLauncher.Plugins;
using CurvaLauncher.Services;
using CurvaLauncher.Utilities.Resources;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace CurvaLauncher.Views.Components;

public class PluginOption : UserControl
{
    static PluginOption()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(PluginOption), new FrameworkPropertyMetadata(typeof(PluginOption)));
    }

    public PluginOption(IPlugin plugin, string optionName, string? optionDescription, string optionPropertyName)
    {
        Plugin = plugin;
        OptionName = optionName;
        OptionDescription = optionDescription;
        OptionPropertyName = optionPropertyName;
    }

    public PluginOption(Assembly resourceAssembly, IPlugin plugin, object optionNameKey, object? optionDescriptionKey, string optionPropertyName)
    {
        Plugin = plugin;
        OptionPropertyName = optionPropertyName;

        SetResourceReference(OptionNameProperty, new AssemblyResourceKey(resourceAssembly, optionNameKey));
        SetResourceReference(OptionDescriptionProperty, new AssemblyResourceKey(resourceAssembly, optionDescriptionKey));
    }

    public IPlugin Plugin { get; }
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
        DependencyProperty.Register(nameof(OptionDescription), typeof(string), typeof(PluginOption), new PropertyMetadata(null));
}
