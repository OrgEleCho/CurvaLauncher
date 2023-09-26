using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MicaLauncher
{
    public partial class AppConfig : ObservableObject
    {
        [ObservableProperty]
        private int _launcherWidth = 800;

        [ObservableProperty]
        private int _queryResultIconSize = 64;
    }
}
