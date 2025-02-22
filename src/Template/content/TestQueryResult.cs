using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.MyCurvaLauncherPlugin;

public class TestQueryResult : IAsyncActionQueryResult
{
    public float Weight { get; }

    public string Title { get; }

    public string Description { get; }

    public ImageSource? Icon => null;

    public TestQueryResult(string title, string description, float weight)
    {
        Title = title;
        Description = description;
        Weight = weight;
    }

    public async Task InvokeAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(2000, cancellationToken);

        MessageBox.Show(Description, Title);
    }
}