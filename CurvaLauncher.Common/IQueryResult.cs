using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Data;

public abstract class QueryResult
{
    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract float Weight { get; }
    public abstract ImageSource? Icon { get; }
}
