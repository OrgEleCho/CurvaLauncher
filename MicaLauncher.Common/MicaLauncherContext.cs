using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MicaLauncher
{
    public class MicaLauncherContext
    {
        public MicaLauncherContext(Dispatcher dispatcher, int requiredIconSize)
        {
            Dispatcher = dispatcher;
            RequiredIconSize = requiredIconSize;
        }

        public Dispatcher Dispatcher { get; }
        public int RequiredIconSize { get; }
    }
}
