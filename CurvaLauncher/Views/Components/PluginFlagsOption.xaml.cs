using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CurvaLauncher.Plugins;

namespace CurvaLauncher.Views.Components
{
    /// <summary>
    /// Interaction logic for PluginFlagsOption.xaml
    /// </summary>
    public partial class PluginFlagsOption : PluginOption
    {
        readonly List<CheckBox> _allInputs = new();

        public PluginFlagsOption(
            IPlugin plugin,
            string optionName,
            string? optionDescription,
            PropertyInfo optionProperty,
            Type enumType) : base(plugin, optionName, optionDescription, optionProperty.Name)
        {
            InitializeComponent();

            BuildOptionControls(optionProperty, enumType);
            UpdateInputs(optionProperty.GetValue(plugin)!);
        }

        public PluginFlagsOption(
            Assembly resourceAssembly,
            IPlugin plugin,
            object optionNameKey,
            object? optionDescriptionKey,
            PropertyInfo optionProperty,
            Type enumType) : base(resourceAssembly, plugin, optionNameKey, optionDescriptionKey, optionProperty.Name)
        {
            InitializeComponent();

            BuildOptionControls(optionProperty, enumType);
            UpdateInputs(optionProperty.GetValue(plugin)!);
        }

        void BuildOptionControls(PropertyInfo optionProperty, Type enumType)
        {

            foreach (var value in Enum.GetValues(enumType))
            {
                var enumName = Enum.GetName(enumType, value);
                var enumValue = (Enum)value;
                dynamic dynamicValue = Convert.ChangeType(value, enumType);

                var checkbox = new CheckBox();
                checkbox.Tag = dynamicValue;
                checkbox.Content = enumName;

                checkbox.Checked += (s, e) =>
                {
                    dynamic currentOptionValue = optionProperty.GetValue(Plugin)!;
                    dynamic newOptionValue = currentOptionValue | dynamicValue;  // set flag

                    optionProperty.SetValue(Plugin, newOptionValue);
                    UpdateInputs(newOptionValue);
                };

                checkbox.Unchecked += (s, e) =>
                {
                    dynamic currentOptionValue = optionProperty.GetValue(Plugin)!;
                    dynamic newOptionValue = currentOptionValue & ~dynamicValue;  // clear flag

                    optionProperty.SetValue(Plugin, newOptionValue);
                    UpdateInputs(newOptionValue);
                };

                _allInputs.Add(checkbox);
            }

            if (Plugin is INotifyPropertyChanged observablePlugin)
            {
                observablePlugin.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == optionProperty.Name)
                    {
                        var newFlagsValue = optionProperty.GetValue(Plugin)!;

                        UpdateInputs(newFlagsValue);
                    }
                };
            }

            foreach (var input in _allInputs)
            {
                inputPanel.Children.Add(input);
            }

            for (int i = 0; i < _allInputs.Count; i++)
            {
                _allInputs[i].Margin = new Thickness(0, 3, 15, 3);
            }
        }

        void UpdateInputs(object newFlagsValue)
        {
            foreach (var input in _allInputs)
            {
                Enum enumnewFlagsValue = (Enum)newFlagsValue;
                dynamic dynamicNewFlagValue = newFlagsValue;
                dynamic dynamicFlag = input.Tag;

                if (enumnewFlagsValue.HasFlag(dynamicFlag))
                {
                    input.IsChecked = true;
                }
                else if ((dynamicNewFlagValue & dynamicFlag) != 0)
                {
                    input.IsChecked = null;
                }
                else
                {
                    input.IsChecked = false;
                }
            }
        }
    }
}
