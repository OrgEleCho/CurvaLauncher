using System;
using System.Runtime.InteropServices;

namespace CurvaLauncher.Utilities;

static class ClipboardUtils
{
    [DllImport("User32")]
    static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("User32")]
    static extern bool CloseClipboard();

    [DllImport("User32")]
    static extern bool EmptyClipboard();

    [DllImport("User32")]
    static extern bool IsClipboardFormatAvailable(int format);

    [DllImport("User32")]
    static extern IntPtr GetClipboardData(int uFormat);

    [DllImport("User32", CharSet = CharSet.Unicode)]
    static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem);

    /// <summary>
    /// 向剪贴板中添加文本
    /// </summary>
    /// <param name="text">文本</param>
    public static void SetText(string text)
    {
        CoreSetText(text, 3);

        void CoreSetText(string text, int remainCount)
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                if (remainCount == 0)
                    throw new InvalidOperationException("Failed to set clipboard text");

                CoreSetText(text, 2);
                return;
            }

            EmptyClipboard();
            SetClipboardData(13, Marshal.StringToHGlobalUni(text));
            CloseClipboard();
        }
    }
}