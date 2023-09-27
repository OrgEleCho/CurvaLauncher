using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MicaLauncher.Utilities
{
    public static class NativeMethods
    {
        public enum DwmWindowAttribute : uint
        {
            NCRenderingEnabled = 1,
            NCRenderingPolicy,
            TransitionsForceDisabled,
            AllowNCPaint,
            CaptionButtonBounds,
            NonClientRtlLayout,
            ForceIconicRepresentation,
            Flip3DPolicy,
            ExtendedFrameBounds,
            HasIconicBitmap,
            DisallowPeek,
            ExcludedFromPeek,
            Cloak,
            Cloaked,
            FreezeRepresentation,
            PassiveUpdateMode,
            UseHostBackdropBrush,
            UseImmersiveDarkMode = 20,
            WindowCornerPreference = 33,
            BorderColor,
            CaptionColor,
            TextColor,
            VisibleFrameBorderThickness,
            SystemBackdropType,
            Last
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute attr, ref int attrValue, int attrSize);

        public static bool EnableDarkModeForWindow(IntPtr hWnd, bool enable)
        {
            int darkMode = enable ? 1 : 0;
            int hr = DwmSetWindowAttribute(hWnd, DwmWindowAttribute.UseImmersiveDarkMode, ref darkMode, sizeof(int));
            return hr >= 0;
        }


        [DllImport("User32.dll", EntryPoint = "MessageBoxW",
                   ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
        public extern static MessageBoxResult MessageBox(IntPtr hwnd, string text, string caption, MessageBoxFlags flags);
    }

    public enum MessageBoxResult : int
    {
        OK = 1,
        Cancel = 2,
        Abort = 3,
        Retry = 4,
        Ignore = 5,
        Yes = 6,
        No = 7,
        Close = 8,
        Help = 9,
        TryAgain = 10,
        Continue = 11,
        Timeout = 32000
    }

    public enum MessageBoxFlags : uint
    {
        AbortRetryIgnore  = 0x2,
        CancelTryContinue = 0x6,
        Help              = 0x4000,
        Ok                = 0x0,
        OkCancel          = 0x1,
        RetryCancel       = 0x5,
        YesNo             = 0x4,
        YesNoCancel       = 0x3,

        IconExclamation   = 0x30,
        IconWarning       = 0x30,
        IconInformation   = 0x40,
        IconAsterisk      = 0x40,
        IconQuestion      = 0x20,
        IconStop          = 0x10,
        IconError         = 0x10,
        IconHand          = 0x10,

        DefButton1        = 0x0,
        DefButton2        = 0x100,
        DefButton3        = 0x200,
        DefButton4        = 0x300,

        ApplModal         = 0x0,
        SystemModal       = 0x1000,
        TaskModal         = 0x2000,

        DefaultDesktopOnly  = 0x20000,
        Right               = 0x80000,
        RtlReading          = 0x100000,
        SetForeground       = 0x10000,
        Topmost             = 0x40000,
        ServiceNotification = 0x200000,
    }
}