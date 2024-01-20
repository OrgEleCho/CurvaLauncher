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
using System.Windows.Shapes;
using CurvaLauncher.Apis;
using Wpf.Ui.Controls;
using static System.Windows.Forms.Design.AxImporter;

namespace CurvaLauncher.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SimpleTextDialog.xaml
    /// </summary>
    public partial class SimpleTextDialog : UiWindow
    {
        public SimpleTextDialog(string text , Apis.TextOptions options)
        {
            Text = text;
            InitializeComponent();

            _options = options;
        }

        public string Text { get; }

        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SizeToContent = SizeToContent.WidthAndHeight;


            Left = (SystemParameters.PrimaryScreenWidth - ActualWidth) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - ActualHeight) / 2;
        }

        private void UiWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        bool _isClosing = false;
        private readonly Apis.TextOptions _options;

        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _isClosing = true;
        }

        private void UiWindow_Deactivated(object sender, EventArgs e)
        {
            if (!_options.HasFlag(Apis.TextOptions.NoAutoClose) && !_isClosing)
                Close();
        }

        private void UiWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && !_isClosing)
                Close();
        }
    }
}
