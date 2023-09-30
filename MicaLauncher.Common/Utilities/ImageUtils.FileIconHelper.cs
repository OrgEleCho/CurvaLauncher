using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MicaLauncher.Utilities
{
    public static partial class ImageUtils
    {
        private unsafe class FileIconHelper
        {
            private delegate bool EnumResNameDelegate(IntPtr hModule, IntPtr lpszType, IntPtr lpszName, IntPtr lParam);
            [DllImport("kernel32.dll", EntryPoint = "EnumResourceNamesW", CharSet = CharSet.Unicode, SetLastError = true)]
            static extern bool EnumResourceNamesWithID(IntPtr hModule, uint lpszType, EnumResNameDelegate lpEnumFunc, IntPtr lParam);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hFile, uint dwFlags);

            [DllImport("kernel32.dll", SetLastError = true)]
            static extern bool FreeLibrary(IntPtr hModule);
            private const uint GROUP_ICON = 14;
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            static extern IntPtr LoadImage(IntPtr hinst, IntPtr lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            extern static bool DestroyIcon(IntPtr handle);

            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            struct SHFILEINFO
            {
                public IntPtr hIcon;
                public int iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            }

            [Flags]
            enum SHGFI : uint
            {
                /// <summary>get icon</summary>
                Icon         = 0x000000100,
                /// <summary>get display name</summary>
                DisplayName      = 0x000000200,
                /// <summary>get type name</summary>
                TypeName     = 0x000000400,
                /// <summary>get attributes</summary>
                Attributes       = 0x000000800,
                /// <summary>get icon location</summary>
                IconLocation     = 0x000001000,
                /// <summary>return exe type</summary>
                ExeType      = 0x000002000,
                /// <summary>get system icon index</summary>
                SysIconIndex     = 0x000004000,
                /// <summary>put a link overlay on icon</summary>
                LinkOverlay      = 0x000008000,
                /// <summary>show icon in selected state</summary>
                Selected     = 0x000010000,
                /// <summary>get only specified attributes</summary>
                Attr_Specified   = 0x000020000,
                /// <summary>get large icon</summary>
                LargeIcon    = 0x000000000,
                /// <summary>get small icon</summary>
                SmallIcon    = 0x000000001,
                /// <summary>get open icon</summary>
                OpenIcon     = 0x000000002,
                /// <summary>get shell size icon</summary>
                ShellIconSize     = 0x000000004,
                /// <summary>pszPath is a pidl</summary>
                PIDL         = 0x000000008,
                /// <summary>use passed dwFileAttribute</summary>
                UseFileAttributes= 0x000000010,
                /// <summary>apply the appropriate overlays</summary>
                AddOverlays      = 0x000000020,
                /// <summary>Get the index of the overlay in the upper 8 bits of the iIcon</summary>
                OverlayIndex     = 0x000000040,
            }

            static uint szSHFILEINFO = (uint)Marshal.SizeOf<SHFILEINFO>();

            public static ImageSource? GetEmbededIconImage(string path, int iconSize)
            {

                // https://github.com/CoenraadS/Windows-Control-Panel-Items/
                // https://gist.github.com/jnm2/79ed8330ceb30dea44793e3aa6c03f5b

                string iconStringRaw = path;
                var iconString = new List<string>(iconStringRaw.Split(new[] { ',' }, 2));
                IntPtr iconPtr = IntPtr.Zero;
                IntPtr dataFilePointer;
                IntPtr iconIndex;
                uint LOAD_LIBRARY_AS_DATAFILE = 0x00000002;

                if (string.IsNullOrEmpty(iconString[0]))
                {
                    //var e = new ArgumentException($"iconString empth {path}");
                    //e.Data.Add(nameof(path), path);

                    return null;
                }

                if (iconString[0][0] == '@')
                {
                    iconString[0] = iconString[0].Substring(1);
                }

                dataFilePointer = LoadLibraryEx(iconString[0], IntPtr.Zero, LOAD_LIBRARY_AS_DATAFILE);
                if (iconString.Count == 2)
                {
                    // C:\WINDOWS\system32\mblctr.exe,0
                    // %SystemRoot%\System32\FirewallControlPanel.dll,-1
                    var index = Math.Abs(int.Parse(iconString[1]));
                    iconIndex = (IntPtr)index;
                    iconPtr = LoadImage(dataFilePointer, iconIndex, 1, iconSize, iconSize, 0);
                }

                if (iconPtr == IntPtr.Zero)
                {
                    IntPtr defaultIconPtr = IntPtr.Zero;
                    var callback = new EnumResNameDelegate((hModule, lpszType, lpszName, lParam) =>
                    {
                        defaultIconPtr = lpszName;
                        return false;
                    });
                    var result = EnumResourceNamesWithID(dataFilePointer, GROUP_ICON, callback, IntPtr.Zero); //Iterate through resources. 
                    if (!result)
                    {
                        int error = Marshal.GetLastWin32Error();
                        int userStoppedResourceEnumeration = 0x3B02;
                        if (error != userStoppedResourceEnumeration)
                        {
                            //Win32Exception exception = new Win32Exception(error);
                            //exception.Data.Add(nameof(path), path);
                            //throw exception;

                            return null;
                        }
                    }
                    iconPtr = LoadImage(dataFilePointer, defaultIconPtr, 1, iconSize, iconSize, 0);
                }

                FreeLibrary(dataFilePointer);
                BitmapSource image;
                if (iconPtr != IntPtr.Zero)
                {
                    image = Imaging.CreateBitmapSourceFromHIcon(iconPtr, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    image.CloneCurrentValue(); //Remove pointer dependancy.
                    image.Freeze();
                    DestroyIcon(iconPtr);
                    return image;
                }
                else
                {
                    //var e = new ArgumentException($"iconPtr zero {path}");
                    //e.Data.Add(nameof(path), path);
                    //throw e;

                    return null;
                }
            }

            public static ImageSource? GetAssociatedIconImage(string path, bool large)
            {
                SHFILEINFO shFileInfo = new();
                SHGFI flags = SHGFI.Icon;

                if (large)
                    flags |= SHGFI.LargeIcon;

                SHGetFileInfo(path, 0, ref shFileInfo, szSHFILEINFO, flags);

                IntPtr hIcon = shFileInfo.hIcon;

                BitmapSource image;
                if (hIcon != IntPtr.Zero)
                {
                    image = Imaging.CreateBitmapSourceFromHIcon(hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    image.CloneCurrentValue(); //Remove pointer dependancy.
                    image.Freeze();
                    DestroyIcon(hIcon);
                    return image;
                }
                else
                {
                    //var e = new ArgumentException($"iconPtr zero {path}");
                    //e.Data.Add(nameof(path), path);
                    //throw e;

                    return null;
                }
            }
        }
    }
}
