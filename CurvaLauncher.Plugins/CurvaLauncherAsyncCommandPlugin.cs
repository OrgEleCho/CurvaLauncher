using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugin;

public abstract class CurvaLauncherAsyncCommandPlugin : CurvaLauncherCommandPlugin
{
    protected CurvaLauncherAsyncCommandPlugin(CurvaLauncherContext context) : base(context)
    {
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;
    public virtual Task FinishAsync() => Task.CompletedTask;

    public abstract IAsyncEnumerable<IQueryResult> ExecuteCommandAsync(CurvaLauncherContext context, string commandName, CommandLineSegment[] arguments);

    public async IAsyncEnumerable<IQueryResult> QueryAsync(CurvaLauncherContext context, string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            yield break;
        if (!query.StartsWith(Prefix))
            yield break;

        var commandLine = query.Substring(Prefix.Length);

        HostContext.CommandLineApi.Split(commandLine, out var segments);

        if (segments.Count < 1)
            yield break;

        var commandNameSegment = segments[0];

        if (commandNameSegment.IsQuoted)
            yield break;

        var argumentSegments = segments
            .Skip(1)
            .ToArray();

        foreach (var commandName in CommandNames)
        {
            if (commandName.Equals(commandNameSegment.Value, IgnoreCases ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCultureIgnoreCase))
            {
                await foreach (var result in ExecuteCommandAsync(context, commandName, argumentSegments))
                    yield return result;

                yield break;
            }
        }
    }
}