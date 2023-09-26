using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MicaLauncher.Data
{
    public abstract class QueryResult
    {
        public abstract float Match { get; }
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract ImageSource? Icon { get; }
        public abstract void Invoke();
    }
}
