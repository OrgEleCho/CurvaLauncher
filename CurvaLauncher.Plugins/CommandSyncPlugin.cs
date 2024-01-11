using CurvaLauncher.Apis;

namespace CurvaLauncher.Plugin;

public abstract class CommandSyncPlugin : CommandPlugin
{
    protected CommandSyncPlugin(CurvaLauncherContext context) : base(context)
    {
    }

    public virtual void Initialize() { }
    public virtual void Finish() { }

    public abstract IEnumerable<IQueryResult> ExecuteCommand(CurvaLauncherContext context, string commandName, CommandLineSegment[] arguments);

    public IEnumerable<IQueryResult> Query(CurvaLauncherContext context, string query)
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
                foreach (var result in ExecuteCommand(context, commandName, argumentSegments))
                    yield return result;

                yield break;
            }
        }
    }
}
