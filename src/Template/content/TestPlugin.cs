using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.MyCurvaLauncherPlugin;

public class TestPlugin : SyncPlugin
{
    [PluginOption]
    public int ResultCount { get; set; } = 5;

    [PluginOption]
    public string Title { get; set; } = "Test";

    public override ImageSource Icon { get; }

    public override string Name => "MyCurvaLauncherPlugin";

    public override string Description { get; } = "A plugin generated from template.";

    public TestPlugin(CurvaLauncherContext context) : base(context)
    {
        Icon = null!;
    }

    public override IEnumerable<IQueryResult> Query(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            yield break;

        for (int i = 0; i < ResultCount; i++)
        {
            yield return new TestQueryResult($"{Title} {i}", $"#{i} of the query result.", (float)i / ResultCount);
        }
    }

    public override void Initialize()
    {
        Debug.WriteLine("MyCurvaLauncherPlugin loaded");
    }

    public override void Finish()
    {
        Debug.WriteLine("MyCurvaLauncherPlugin Unloaded");
    }
}