using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CurvaLauncher
{
    public class CurvaLauncherContext
    {
        public CurvaLauncherContext(Dispatcher dispatcher, int requiredIconSize)
        {
            Dispatcher = dispatcher;
            RequiredIconSize = requiredIconSize;
        }

        public Dispatcher Dispatcher { get; }
        public int RequiredIconSize { get; }

        public bool Alternate { get; set; }
    }
}
