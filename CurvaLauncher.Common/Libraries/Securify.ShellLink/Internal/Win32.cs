using System;
using System.Runtime.InteropServices;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Internal
{
    /// <summary>
    /// Win32 interop
    /// </summary>
    class Win32
    {
        /// <summary>
        ///  The maximum length for a path is MAX_PATH, which is defined as 260 characters
        /// </summary>
        public const int MAX_PATH = 260;

        /// <summary>
        /// Converts an item identifier list to a file system path. This function extends SHGetPathFromIDList 
        /// by allowing you to set the initial size of the string buffer and declare the options below.
        /// </summary>
        /// <param name="pidl">A pointer to an item identifier list that specifies a file or directory location 
        /// relative to the root of the namespace (the desktop).</param>
        /// <param name="pszPath">The address of a buffer to receive the file system path. This buffer must be 
        /// at least MAX_PATH characters in size.</param>
        /// <returns>Returns TRUE if successful; otherwise, FALSE.</returns>
        [DllImport("shell32.dll")]
        public static extern bool SHGetPathFromIDListW(byte[] pidl, byte[] pszPath);

        /// <summary>
        /// Retrieves the display name of an item identified by its IDList.
        /// </summary>
        /// <param name="pidl">A PIDL that identifies the item.</param>
        /// <param name="sigdnName">A value from the SIGDN enumeration that specifies the type of display name 
        /// to retrieve.</param>
        /// <param name="ppszName">A value that, when this function returns successfully, receives the address 
        /// of a pointer to the retrieved display name.</param>
        /// <returns></returns>
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern uint SHGetNameFromIDList(byte[] pidl, SIGDN sigdnName, out IntPtr ppszName);

        /// <summary>
        /// Retrieves the display name of an item identified by its IDList.
        /// </summary>
        /// <param name="pidl">A PIDL that identifies the item.</param>
        /// <param name="sigdnName">A value from the SIGDN enumeration that specifies the type of display name 
        /// to retrieve.</param>
        /// <param name="ppszName">A value that, when this function returns successfully, receives the address 
        /// of a pointer to the retrieved display name.</param>
        /// <returns></returns>
        [DllImport("shell32.dll", SetLastError = true)]
        public static extern uint SHGetNameFromIDList(IntPtr pidl, SIGDN sigdnName, out IntPtr ppszName);

        /// <summary>
        /// Retrieves the path of a known folder as an ITEMIDLIST structure.
        /// </summary>
        /// <param name="rfid">A reference to the KNOWNFOLDERID that identifies the folder. The folders associated 
        /// with the known folder IDs might not exist on a particular system.</param>
        /// <param name="dwFlags">Flags that specify special retrieval options. This value can be 0; otherwise, 
        /// it is one or more of the KNOWN_FOLDER_FLAG values.></param>
        /// <param name="hToken">An access token used to represent a particular user. This parameter is usually 
        /// set to NULL, in which case the function tries to access the current user's instance of the folder. 
        /// However, you may need to assign a value to hToken for those folders that can have multiple users but
        /// are treated as belonging to a single user. The most commonly used folder of this type is Documents.
        /// 
        /// The calling application is responsible for correct impersonation when hToken is non-null. It must have 
        /// appropriate security privileges for the particular user, including TOKEN_QUERY and TOKEN_IMPERSONATE, 
        /// and the user's registry hive must be currently mounted. See Access Control for further discussion of 
        /// access control issues.
        /// 
        /// Assigning the hToken parameter a value of -1 indicates the Default User.This allows clients of 
        /// SHGetKnownFolderIDList to find folder locations (such as the Desktop folder) for the Default User.
        /// The Default User user profile is duplicated when any new user account is created, and includes special 
        /// folders such as Documents and Desktop.Any items added to the Default User folder also appear in any 
        /// new user account.Note that access to the Default User folders requires administrator privileges.</param>
        /// <param name="ppidl">When this method returns, contains a pointer to the PIDL of the folder. 
        /// This parameter is passed uninitialized. The caller is responsible for freeing the returned PIDL when 
        /// it is no longer needed by calling ILFree.</param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern int SHGetKnownFolderIDList([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppidl);

        /// <summary>
        /// Frees a block of task memory previously allocated through a call to the CoTaskMemAlloc or 
        /// CoTaskMemRealloc function.
        /// </summary>
        /// <param name="pv">A pointer to the memory block to be freed. If this parameter is NULL, the function 
        /// has no effect.</param>
        [DllImport("ole32.dll")]
        public static extern void CoTaskMemFree(IntPtr pv);

        /// <summary>
        /// Returns the ITEMIDLIST structure associated with a specified file path.
        /// </summary>
        /// <param name="pszPath">A pointer to a null-terminated Unicode string that contains the path. 
        /// This string should be no more than MAX_PATH characters in length, including the terminating null 
        /// character.</param>
        /// <returns>Returns a pointer to an ITEMIDLIST structure that corresponds to the path.</returns>
        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr ILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath);

        /// <summary>
        /// Frees an ITEMIDLIST structure allocated by the Shell.
        /// </summary>
        /// <param name="pidl">A pointer to the ITEMIDLIST structure to be freed. This parameter can be NULL.
        /// </param>
        [DllImport("shell32.dll")]
        public static extern void ILFree(IntPtr pidl);

        /// <summary>
        /// Returns a pointer to an ITEMIDLIST structure when passed a path (Deprecated).
        /// </summary>
        /// <param name="pszPath">A pointer to a null-terminated string that contains the path to be converted to a PIDL.</param>
        /// <returns>Returns a pointer to an ITEMIDLIST structure if successful, or NULL otherwise.</returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHSimpleIDListFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath);
    }

    enum SIGDN : uint
    {
        /// <summary>
        /// 0x00000000. Returns the display name relative to the parent folder. In UI this name is generally 
        /// ideal for display to the user.
        /// </summary>
        SIGDN_NORMALDISPLAY = 0x00000000,
        /// <summary>
        /// (int)0x80018001. Returns the parsing name relative to the parent folder. This name is not suitable 
        /// for use in UI.
        /// </summary>
        SIGDN_PARENTRELATIVEPARSING = 0x80018001,
        /// <summary>
        /// (int)0x80028000. Returns the parsing name relative to the desktop. This name is not suitable for
        /// use in UI.
        /// </summary>
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
        /// <summary>
        /// (int)0x80031001. Returns the editing name relative to the parent folder. In UI this name is suitable 
        /// for display to the user.
        /// </summary>
        SIGDN_PARENTRELATIVEEDITING = 0x80031001,
        /// <summary>
        /// (int)0x8004c000. Returns the editing name relative to the desktop. In UI this name is suitable for 
        /// display to the user.
        /// </summary>
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
        /// <summary>
        /// (int)0x80058000. Returns the item's file system path, if it has one. Only items that report 
        /// SFGAO_FILESYSTEM have a file system path. When an item does not have a file system path, a call
        /// to IShellItem::GetDisplayName on that item will fail. In UI this name is suitable for display to the
        /// user in some cases, but note that it might not be specified for all items.
        /// </summary>
        SIGDN_FILESYSPATH = 0x80058000,
        /// <summary>
        /// (int)0x80068000. Returns the item's URL, if it has one. Some items do not have a URL, and in those 
        /// cases a call to IShellItem::GetDisplayName will fail. This name is suitable for display to the user
        /// in some cases, but note that it might not be specified for all items.
        /// </summary>
        SIGDN_URL = 0x80068000,
        /// <summary>
        /// (int)0x8007c001. Returns the path relative to the parent folder in a friendly format as displayed in 
        /// an address bar. 
        /// This name is suitable for display to the user.
        /// </summary>
        SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
        /// <summary>
        /// (int)0x80080001. Returns the path relative to the parent folder.
        /// </summary>
        SIGDN_PARENTRELATIVE = 0x80080001,
        /// <summary>
        /// (int)0x80094001. Introduced in Windows 8.
        /// </summary>
        SIGDN_PARENTRELATIVEFORUI = 0x80094001
    }
}
