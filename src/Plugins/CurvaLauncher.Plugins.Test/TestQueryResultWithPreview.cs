using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Test
{
    public class TestQueryResultWithPreview : IAsyncActionQueryResult, IQueryResultWithPreview
    {
        private readonly FlowDocument _preview;

        public TestQueryResultWithPreview(string title, string description, float weight, FlowDocument preview)
        {
            Title = title;
            Description = description;
            Weight = weight;
            this._preview = preview;
        }

        public float Weight { get; }

        public string Title { get; }

        public string Description { get; }

        public ImageSource? Icon => null;

        public FlowDocument GeneratePreview() => _preview;

        public async Task InvokeAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);

            MessageBox.Show(Description, Title);
        }
    }

}