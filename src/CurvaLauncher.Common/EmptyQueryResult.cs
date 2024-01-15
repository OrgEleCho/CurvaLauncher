using System.Windows.Media;

namespace CurvaLauncher;

public class EmptyQueryResult : IQueryResult
{
    private readonly string _title;
    private readonly string _description;
    private readonly float _weight;
    private readonly ImageSource? _icon;

    public float Weight => _weight;
    public string Title => _title;
    public string Description => _description;

    public ImageSource? Icon => _icon;

    public EmptyQueryResult(string title, string description, float weight, ImageSource? icon)
    {
        _title = title;
        _description = description;
        _weight = weight;
        _icon = icon;
    }
}
