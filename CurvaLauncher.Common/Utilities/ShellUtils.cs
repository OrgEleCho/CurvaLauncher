using System.Diagnostics;

namespace CurvaLauncher.Utilities
{
    public static class ShellUtils
    {
        public static void Start(string? address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return;

            Process.Start(
                new ProcessStartInfo()
                {
                    FileName = address,
                    UseShellExecute = true,
                });
        }
    }
}
