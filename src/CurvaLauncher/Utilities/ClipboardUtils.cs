using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CurvaLauncher.Utilities;

static class ClipboardUtils
{
    [DllImport("User32")]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("User32")]
    private static extern bool CloseClipboard();

    [DllImport("User32")]
    private static extern bool EmptyClipboard();

    [DllImport("User32")]
    private static extern bool IsClipboardFormatAvailable(int format);

    [DllImport("User32")]
    private static extern IntPtr GetClipboardData(int uFormat);

    [DllImport("User32", CharSet = CharSet.Unicode)]
    private static extern IntPtr SetClipboardData(int uFormat, IntPtr hMem);

    [DllImport("gdi32.dll")]
    private static extern int GetObject(nint hObject, int c, out nint resultPtr);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(nint hObject);

    [DllImport("gdi32.dll")]
    private static extern nint CreateDIBitmap(nint hdc, in BITMAPINFOHEADER pbmih, uint flInit, nint pjBits, in byte pbmi, uint iUsage);

    [DllImport("user32.dll")]
    private static extern nint GetDC(nint hwnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(nint hwnd, nint hdc);

    static bool CoreIsClipboardFormatAvailable(int format, int remainCount)
    {
        if (!OpenClipboard(IntPtr.Zero))
        {
            if (remainCount == 0)
                throw new InvalidOperationException("Failed to set clipboard text");

            return CoreIsClipboardFormatAvailable(format, remainCount - 1);
        }

        EmptyClipboard();
        bool available = IsClipboardFormatAvailable(format);
        CloseClipboard();

        return available;
    }

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

    public static bool HasText() => CoreIsClipboardFormatAvailable(13, 3) || CoreIsClipboardFormatAvailable(1, 3);

    public static bool HasImage() => CoreIsClipboardFormatAvailable(2, 3);



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

        SetBitmap(hBitmap);
        DeleteObject(hBitmap);
    }

    /// <summary>
    /// 向剪切板中添加图片
    /// </summary>
    /// <param name="hBitmap">位图句柄</param>
    private static unsafe void SetBitmap(nint hBitmap)
    {
        DIBSECTION ds;
        GetObject(hBitmap, sizeof(DIBSECTION), out *(nint*)&ds);

        ds.dsBmih.biCompression = 0;
        var hdc = GetDC(0);
        nint hbitmap_ddb = CreateDIBitmap(hdc, in ds.dsBmih, 0x4/*CBM_INIT*/, ds.dsBm.bmBits, Unsafe.As<BITMAPINFOHEADER, byte>(ref ds.dsBmih), /*DIB_RGB_COLORS*/ 0);
        ReleaseDC(0, hdc);

        CoreSetBitmap(hbitmap_ddb, 3);
        DeleteObject(hbitmap_ddb);
    }





    private struct BITMAP
    {
        public int bmType;
        public int bmWidth;
        public int bmHeight;
        public int bmWidthBytes;
        public ushort bmPlanes;
        public ushort bmBitsPixel;
        public nint bmBits;
    }

    private struct DIBSECTION
    {
        public BITMAP dsBm;
        public BITMAPINFOHEADER dsBmih;
        public uint dsBitfields1;
        public uint dsBitfields2;
        public uint dsBitfields3;
        public nint dshSection;
        public uint dsOffset;
    }

    private struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    private struct BITMAPINFO
    {
        public BITMAPINFOHEADER bmiHeader;
        public RGBQUAD bmiColors;
    }

    private struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }
}