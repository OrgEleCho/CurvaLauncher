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

namespace CurvaLauncher.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for SimpleImageDialog.xaml
    /// </summary>
    public partial class SimpleImageDialog : UiWindow
    {
        private readonly ImageOptions _options;

        public SimpleImageDialog(ImageSource image, Apis.ImageOptions options)
        {
            Image = image;

            InitializeComponent();

            _options = options;
        }

        public ImageSource Image { get; }

        private void UiWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SizeToContent = SizeToContent.WidthAndHeight;


            Left = (SystemParameters.PrimaryScreenWidth - ActualWidth) / 2;
            Top = (SystemParameters.PrimaryScreenHeight - ActualHeight) / 2;
        }

        private void UiWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        bool _isClosing = false;
        private void UiWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _isClosing = true;
        }

        private void UiWindow_Deactivated(object sender, EventArgs e)
        {
            if (!_options.HasFlag(Apis.ImageOptions.NoAutoClose) && !_isClosing)
                Close();
        }

        private void UiWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && !_isClosing)
                Close();
        }
    }
}
