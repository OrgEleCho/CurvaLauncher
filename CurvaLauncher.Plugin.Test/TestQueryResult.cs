using System.Windows;
using System.Windows.Media;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.Test
{
    public class TestQueryResult : AsyncQueryResult
    {
        public TestQueryResult(string title, string description, float weight)
        {
            Title = title;
            Description = description;
            Weight = weight;
        }

        public override float Weight { get; }

        public override string Title { get; }

        public override string Description {get;}

        public override ImageSource? Icon => null;

        public override async Task InvokeAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(2000, cancellationToken);

            MessageBox.Show(Description, Title);
        }
    }
}