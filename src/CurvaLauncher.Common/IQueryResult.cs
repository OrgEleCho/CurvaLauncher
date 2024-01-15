using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher;

public interface IQueryResult
{
    public string Title { get; }
    public string Description { get; }
    public float Weight { get; }
    public ImageSource? Icon { get; }
}
