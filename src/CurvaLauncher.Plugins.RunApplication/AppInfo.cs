namespace CurvaLauncher.Plugins.RunApplication;

public abstract record class AppInfo
{
    private string? _queryRoot;

    public string Name { get; set; } = string.Empty;

    public string QueryRoot { get => _queryRoot ?? Name; set => _queryRoot = value; }

    public string[]? AlterQueryRoots { get; set; }
}