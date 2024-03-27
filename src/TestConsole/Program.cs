using CurvaLauncher.Apis;
using CurvaLauncher.Utilities;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System;
using System.Runtime.InteropServices;


[DllImport("shell32.dll", ExactSpelling = true)]
static extern void ILFree(IntPtr pidlList);

[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
static extern IntPtr ILCreateFromPathW([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

[DllImport("shell32.dll")]
static extern int SHOpenFolderAndSelectItems(IntPtr pidlList, uint cidl, IntPtr[] apidl, uint dwFlags);

static void ShowInFileExplorer(string path)
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

string filePath = @"C:\Users";

// Example usage
try
{
    ShowInFileExplorer(filePath);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

var w = new QRCodeWriter();
var b = w.encode("Fuck you world", BarcodeFormat.QR_CODE, 100, 100);
var writer = new ZXing.Windows.Compatibility.BarcodeWriter()
{
    Format = BarcodeFormat.EAN_13,
    Options = new()
    {
        Width = 100,
        Height = 100,
        Margin = 0,
    }
};

var bmp = writer.Write("Fuck you world");
bmp.Save("QWQ.png");
//zzb.Write

void PinyinTest()
{
    while (true)
    {
        Console.Write(">>> ");
        string? pinyin = Console.ReadLine();

        if (TestConsole.Pinyin.Pronounce.TryParse(pinyin, out var pronounce))
        {
            Console.WriteLine($"结果: 声母{pronounce.Consonant}, 介母:{pronounce.SemiVowel}, 韵母: {pronounce.Vowel}, ToString: {pronounce}");
        }
        else
        {
            Console.WriteLine("无效拼音");
        }
    }
}