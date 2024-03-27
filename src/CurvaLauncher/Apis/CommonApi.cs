using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Media;
using CurvaLauncher.Views.Dialogs;

namespace CurvaLauncher.Apis
{
    public class CommonApi : ICommonApi
    {
        [DllImport("shell32.dll", ExactSpelling = true)]
        static extern void ILFree(IntPtr pidlList);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr ILCreateFromPathW([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

        [DllImport("shell32.dll")]
        static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cidl, IntPtr[]? apidl, uint dwFlags);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern bool SHObjectProperties(IntPtr hwnd, uint shopObjectType, string pszObjectName, string? pszPropertyPage);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern int ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string? lpParameters, string? lpDirectory, int nShowCmd);

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

        public void ShowInFileExplorer(string path)
        {
            IntPtr pidlList = ILCreateFromPathW(path);
            if (pidlList == IntPtr.Zero)
            {
                throw new ArgumentException("Invalid path");
            }

            try
            {
                SHOpenFolderAndSelectItems(pidlList, 0, null, 0);
            }
            finally
            {
                ILFree(pidlList);
            }
        }

        public void ShowPropertiesWindow(string path)
        {
            bool invoked = SHObjectProperties(IntPtr.Zero, 2, path, null);
        }

        public void ShowImage(ImageSource image, ImageOptions options)
        {
            new SimpleImageDialog(image, options)
                .Show();
        }

        public void ShowText(string text, TextOptions options)
        {
            new SimpleTextDialog(text, options)
                .Show();
        }
    }
}
