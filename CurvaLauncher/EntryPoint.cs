using System;
using CurvaLauncher.Utilities;

namespace CurvaLauncher;

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
        NativeMethods.MessageBox(IntPtr.Zero, $"{e.ExceptionObject}", "CurvaLauncher Error", MessageBoxFlags.IconError | MessageBoxFlags.Ok);
    }
}
