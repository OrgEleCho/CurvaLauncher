using System.Diagnostics;
using CurvaLauncher.Utilities;

namespace CurvaLauncher.Apis
{
    public class CommonApi : ICommonApi
    {
        private CommonApi() { }

        public static CommonApi Instance { get; } = new();

        public void Open(string name) => Process.Start(
            new ProcessStartInfo()
            {
                FileName = name,
                UseShellExecute = true,
            });

        public void OpenExecutable(string file) => Process.Start(
            new ProcessStartInfo()
            {
                FileName = file,
                UseShellExecute = false
            });

        public void SetClipboardText(string text)
        {
            ClipboardUtils.SetText(text);
        }
    }
}
