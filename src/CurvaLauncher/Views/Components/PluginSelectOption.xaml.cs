using System;
using System.Collections;
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
    /// Interaction logic for PluginSelectOption.xaml
    /// </summary>
    public partial class PluginSelectOption : PluginOption
    {
        public PluginSelectOption(IPlugin plugin, 
            string optionName, 
            string? optionDescription,
            string optionPropertyName,
            IEnumerable selections) : base(plugin, optionName, optionDescription, optionPropertyName)
        {
            Selections = selections;

            InitializeComponent();
            BuildOptionControls();
        }
        public PluginSelectOption(
            Assembly resourceAssembly,
            IPlugin plugin,
            object optionNameKey,
            object? optionDescriptionKey,
            string optionPropertyName,
            IEnumerable selections) : base(resourceAssembly, plugin, optionNameKey, optionDescriptionKey, optionPropertyName)
        {
            Selections = selections;

            InitializeComponent();
            BuildOptionControls();
        }

        public IEnumerable Selections { get; }

        void BuildOptionControls()
        {
            foreach (var selection in Selections)
                input.Items.Add(selection);

            input.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Source = Plugin,
                Path = new PropertyPath(OptionPropertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }
    }
}
