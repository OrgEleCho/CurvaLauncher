using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicaLauncher.Utilities;

namespace MicaLauncher
{
    public static class EntryPoint
    {
        static EntryPoint()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        [STAThread]
        static void Main()
        {
            App app = new App();

            app.InitializeComponent();
            app.Run();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            NativeMethods.MessageBox(IntPtr.Zero, $"{e.ExceptionObject}", "MicaLauncher Error", MessageBoxFlags.IconError | MessageBoxFlags.Ok);
        }
    }
}
