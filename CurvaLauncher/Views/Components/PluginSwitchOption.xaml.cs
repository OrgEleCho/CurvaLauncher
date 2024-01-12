using System;
using System.Collections.Generic;
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
    /// Interaction logic for PluginSwitchOption.xaml
    /// </summary>
    public partial class PluginSwitchOption : PluginOption
    {
        public PluginSwitchOption(
            IPlugin plugin,
            string optionName,
            string? optionDescription,
            string optionPropertyName)
            : base(plugin, optionName, optionDescription, optionPropertyName)
        {
            InitializeComponent();
            BuildOptionControls();
        }

        public PluginSwitchOption(
            Assembly resourceAssembly,
            IPlugin plugin,
            object optionNameKey,
            object? optionDescriptionKey,
            string optionPropertyName) : base(resourceAssembly, plugin, optionNameKey, optionDescriptionKey, optionPropertyName)
        {
            InitializeComponent();
            BuildOptionControls();
        }

        void BuildOptionControls()
        {
            input.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = Plugin,
                Path = new PropertyPath(OptionPropertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }
    }
}
