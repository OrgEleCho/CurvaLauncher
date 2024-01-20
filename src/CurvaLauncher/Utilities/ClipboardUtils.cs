using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

    static void CoreSetText(string text, int remainCount)
    {
        if (!OpenClipboard(IntPtr.Zero))
        {
            if (remainCount == 0)
                throw new InvalidOperationException("Failed to set clipboard text");

            CoreSetText(text, remainCount - 1);
            return;
        }

        EmptyClipboard();
        SetClipboardData(13, Marshal.StringToHGlobalUni(text));
        CloseClipboard();
    }

    static void CoreSetBitmap(nint hBitmap, int remainCount)
    {
        if (!OpenClipboard(IntPtr.Zero))
        {
            if (remainCount == 0)
                throw new InvalidOperationException("Failed to set clipboard text");

            CoreSetBitmap(hBitmap, remainCount - 1);
            return;
        }

        EmptyClipboard();
        SetClipboardData(2, hBitmap);
        CloseClipboard();
    }

    /// <summary>
    /// 向剪贴板中添加文本
    /// </summary>
    /// <param name="text">文本</param>
    public static void SetText(string text)
    {
        CoreSetText(text, 3);

    }

    /// <summary>
    /// 向剪切板添加图片
    /// </summary>
    /// <param name="imageSource"></param>
    public static void SetBitmap(ImageSource imageSource)
    {
        using MemoryStream ms = new();
        BmpBitmapEncoder encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)imageSource));
        encoder.Save(ms);

        SetBitmap(ms);
    }

    /// <summary>
    /// 向剪切板中添加图片
    /// </summary>
    /// <param name="bitmapStream">图片流</param>
    public static void SetBitmap(Stream bitmapStream)
    {
        using var bmp = new Bitmap(bitmapStream);
        var hBitmap = bmp.GetHbitmap();

        CoreSetBitmap(hBitmap, 3);
    }

    /// <summary>
    /// 向剪切板中添加图片
    /// </summary>
    /// <param name="image">图片</param>
    public static void SetBitmap(Image image)
    {
        bool selfCreated = false;
        if (image is not Bitmap bitmap)
        {
            bitmap = new Bitmap(image);
            selfCreated = true;
        }

        SetBitmap(bitmap);

        if (selfCreated)
            bitmap.Dispose();
    }

    /// <summary>
    /// 向剪切板中添加图片
    /// </summary>
    /// <param name="bitmap">位图</param>
    public static void SetBitmap(Bitmap bitmap)
    {
        var hBitmap = bitmap.GetHbitmap();

        CoreSetBitmap(hBitmap, 3);
    }

    /// <summary>
    /// 向剪切板中添加图片
    /// </summary>
    /// <param name="hBitmap">位图句柄</param>
    public static void SetBitmap(nint hBitmap)
    {
        CoreSetBitmap(hBitmap, 3);
    }
}