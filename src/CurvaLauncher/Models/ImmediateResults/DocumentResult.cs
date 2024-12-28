using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;

namespace CurvaLauncher.Models.ImmediateResults
{
    public class DocumentResult : ImmediateResult
    {
        public FlowDocument Document { get; }

        public DocumentResult(FlowDocument document)
        {
            var fontFamilyLocalValue = document.ReadLocalValue(FlowDocument.FontFamilyProperty);
            if (App.Current.MainWindow is not null)
            {
                if (fontFamilyLocalValue == DependencyProperty.UnsetValue ||
                    fontFamilyLocalValue == Binding.DoNothing)
                {
                    document.FontFamily = App.Current.MainWindow.FontFamily;
                }
            }

            Document = document;
        }
    }
}
