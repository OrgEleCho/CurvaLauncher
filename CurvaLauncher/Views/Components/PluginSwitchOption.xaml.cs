using System;
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
    /// Interaction logic for PluginSwitchOption.xaml
    /// </summary>
    public partial class PluginSwitchOption : PluginOption
    {
        public PluginSwitchOption(
            CurvaLauncherPlugin plugin,
            string optionName,
            string? optionDescription,
            string optionPropertyName)
            : base(plugin, optionName, optionDescription, optionPropertyName)
        {
            InitializeComponent();

            input.SetBinding(CheckBox.IsCheckedProperty, new Binding
            {
                Source = plugin,
                Path = new PropertyPath(optionPropertyName),
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });
        }
    }
}
