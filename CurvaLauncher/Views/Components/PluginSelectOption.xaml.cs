using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
using CurvaLauncher.Plugin;

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

            foreach (var selection in selections)
                input.Items.Add(selection);

            input.SetBinding(ComboBox.SelectedItemProperty, new Binding
            {
                Source = plugin,
                Path = new PropertyPath(optionPropertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }

        public IEnumerable Selections { get; }
    }
}
