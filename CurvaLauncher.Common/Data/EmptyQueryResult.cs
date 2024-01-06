using System.Windows.Media;

namespace CurvaLauncher.Data;

public class EmptyQueryResult : QueryResult
{
    private readonly string _title;
    private readonly string _description;
    private readonly float _weight;
    private readonly ImageSource? _icon;

    public override float Weight => _weight;
    public override string Title => _title;
    public override string Description => _description;

    public override ImageSource? Icon => _icon;

    public EmptyQueryResult(string title, string description, float weight, ImageSource? icon)
    {
        _title = title;
        _description = description;
        _weight = weight;
        _icon = icon;
    }
}
