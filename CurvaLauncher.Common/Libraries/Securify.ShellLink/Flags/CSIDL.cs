using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// CSIDL (constant special item ID list) values provide a unique system-independent way to identify special 
    /// folders used frequently by applications, but which may not have the same name or location on any given 
    /// system. For example, the system folder may be "C:\Windows" on one system and "C:\Winnt" on another. 
    /// </summary>
    public enum CSIDL : uint
    {
        /// <summary>
        /// The virtual folder that represents the Windows desktop, the root of the namespace.
        /// </summary>
        CSIDL_DESKTOP = 0x00,
        /// <summary>
        /// A virtual folder for Internet Explorer.
        /// </summary>
        CSIDL_INTERNET = 0x01,
        /// <summary>
        /// The file system directory that contains the user's program groups (which are themselves 
        /// file system directories). A typical path is C:\Documents and Settings\username\Start Menu\Programs.
        /// </summary>
        CSIDL_PROGRAMS = 0x02,
        /// <summary>
        /// The virtual folder that contains icons for the Control Panel applications.
        /// </summary>
        CSIDL_CONTROLS = 0x03,
        /// <summary>
        /// The virtual folder that contains installed printers.
        /// </summary>
        CSIDL_PRINTERS = 0x04,
        /// <summary>
        /// Version 6.0. The virtual folder that represents the My Documents desktop item. 
        /// This is equivalent to CSIDL_MYDOCUMENTS.
        /// 
        /// Previous to Version 6.0. The file system directory used to physically store a user's common 
        /// repository of documents. A typical path is C:\Documents and Settings\username\My Documents. 
        /// This should be distinguished from the virtual My Documents folder in the namespace. 
        /// To access that virtual folder, use SHGetFolderLocation, which returns the ITEMIDLIST for the 
        /// virtual location, or refer to the technique described in Managing the File System.
        /// </summary>
        CSIDL_PERSONAL = 0x05,
        /// <summary>
        /// The file system directory that serves as a common repository for the user's favorite items. 
        /// A typical path is C:\Documents and Settings\username\Favorites.
        /// </summary>
        CSIDL_FAVORITES = 0x06,
        /// <summary>
        /// The file system directory that corresponds to the user's Startup program group. The system 
        /// starts these programs whenever the associated user logs on. A typical path is 
        /// C:\Documents and Settings\username\Start Menu\Programs\Startup.
        /// </summary>
        CSIDL_STARTUP = 0x07,
        /// <summary>
        /// The file system directory that contains shortcuts to the user's most recently used documents. 
        /// A typical path is C:\Documents and Settings\username\My Recent Documents. To create a shortcut
        /// in this folder, use SHAddToRecentDocs. In addition to creating the shortcut, this function updates
        /// the Shell's list of recent documents and adds the shortcut to the My Recent Documents submenu of 
        /// the Start menu.
        /// </summary>
        CSIDL_RECENT = 0x08,
        /// <summary>
        /// The file system directory that contains Send To menu items. A typical path is 
        /// C:\Documents and Settings\username\SendTo.
        /// </summary>
        CSIDL_SENDTO = 0x09,
        /// <summary>
        /// The virtual folder that contains the objects in the user's Recycle Bin.
        /// </summary>
        CSIDL_BITBUCKET = 0x0A,
        /// <summary>
        /// The file system directory that contains Start menu items. A typical path is 
        /// C:\Documents and Settings\username\Start Menu.
        /// </summary>
        CSIDL_STARTMENU = 0x0B,
        /// <summary>
        /// The virtual folder that represents the My Documents desktop item. This value is equivalent to CSIDL_PERSONAL.
        /// </summary>
        CSIDL_MYDOCUMENTS = 0x0C,
        /// <summary>
        /// The file system directory that serves as a common repository for music files. 
        /// A typical path is C:\Documents and Settings\User\My Documents\My Music.
        /// </summary>
        CSIDL_MYMUSIC = 0x0D,
        /// <summary>
        /// The file system directory that serves as a common repository for video files. 
        /// A typical path is C:\Documents and Settings\username\My Documents\My Videos.
        /// </summary>
        CSIDL_MYVIDEO = 0x0E,
        /// <summary>
        /// The file system directory used to physically store file objects on the desktop (not to be confused 
        /// with the desktop folder itself). A typical path is C:\Documents and Settings\username\Desktop.
        /// </summary>
        CSIDL_DESKTOPDIRECTORY = 0x10,
        /// <summary>
        /// The virtual folder that represents My Computer, containing everything on the local computer: 
        /// storage devices, printers, and Control Panel. The folder can also contain mapped network drives.
        /// </summary>
        CSIDL_DRIVES = 0x11,
        /// <summary>
        /// A virtual folder that represents Network Neighborhood, the root of the network namespace hierarchy.
        /// </summary>
        CSIDL_NETWORK = 0x12,
        /// <summary>
        /// A file system directory that contains the link objects that may exist in the My Network Places 
        /// virtual folder. It is not the same as CSIDL_NETWORK, which represents the network namespace root. 
        /// A typical path is C:\Documents and Settings\username\NetHood.
        /// </summary>
        CSIDL_NETHOOD = 0x13,
        /// <summary>
        /// A virtual folder that contains fonts. A typical path is C:\Windows\Fonts.
        /// </summary>
        CSIDL_FONTS = 0x14,
        /// <summary>
        /// The file system directory that serves as a common repository for document templates. 
        /// A typical path is C:\Documents and Settings\username\Templates.
        /// </summary>
        CSIDL_TEMPLATES = 0x15,
        /// <summary>
        /// The file system directory that contains the programs and folders that appear on the Start menu for 
        /// all users. A typical path is C:\Documents and Settings\All Users\Start Menu.
        /// </summary>
        CSIDL_COMMON_STARTMENU = 0x16,
        /// <summary>
        /// The file system directory that contains the directories for the common program groups that 
        /// appear on the Start menu for all users. A typical path is 
        /// C:\Documents and Settings\All Users\Start Menu\Programs.
        /// </summary>
        CSIDL_COMMON_PROGRAMS = 0x17,
        /// <summary>
        /// The file system directory that contains the programs that appear in the Startup folder for 
        /// all users. A typical path is C:\Documents and Settings\All Users\Start Menu\Programs\Startup.
        /// </summary>
        CSIDL_COMMON_STARTUP = 0x18,
        /// <summary>
        /// The file system directory that contains files and folders that appear on the desktop for all 
        /// users. A typical path is C:\Documents and Settings\All Users\Desktop.
        /// </summary>
        CSIDL_COMMON_DESKTOPDIRECTORY = 0x19,
        /// <summary>
        /// The file system directory that serves as a common repository for application-specific data.
        /// A typical path is C:\Documents and Settings\username\Application Data.
        /// </summary>
        CSIDL_APPDATA = 0x1A,
        /// <summary>
        /// The file system directory that contains the link objects that can exist in the Printers virtual 
        /// folder. A typical path is C:\Documents and Settings\username\PrintHood.
        /// </summary>
        CSIDL_PRINTHOOD = 0x1B,
        /// <summary>
        /// The file system directory that serves as a data repository for local (nonroaming) applications. 
        /// A typical path is C:\Documents and Settings\username\Local Settings\Application Data.
        /// </summary>
        CSIDL_LOCAL_APPDATA = 0x1C,
        /// <summary>
        /// The file system directory that corresponds to the user's nonlocalized Startup program group. 
        /// This value is recognized in Windows Vista for backward compatibility, but the folder itself
        /// no longer exists.
        /// </summary>
        CSIDL_ALTSTARTUP = 0x1D,
        /// <summary>
        /// The file system directory that corresponds to the nonlocalized Startup program group for all 
        /// users. This value is recognized in Windows Vista for backward compatibility, but the folder 
        /// itself no longer exists.
        /// </summary>
        CSIDL_COMMON_ALTSTARTUP = 0x1E,
        /// <summary>
        /// The file system directory that serves as a common repository for favorite items common to all 
        /// users.
        /// </summary>
        CSIDL_COMMON_FAVORITES = 0x1F,
        /// <summary>
        /// The file system directory that serves as a common repository for temporary Internet files. 
        /// A typical path is C:\Documents and Settings\username\Local Settings\Temporary Internet Files.
        /// </summary>
        CSIDL_INTERNET_CACHE = 0x20,
        /// <summary>
        /// The file system directory that serves as a common repository for Internet cookies. 
        /// A typical path is C:\Documents and Settings\username\Cookies.
        /// </summary>
        CSIDL_COOKIES = 0x21,
        /// <summary>
        /// The file system directory that serves as a common repository for Internet history items.
        /// </summary>
        CSIDL_HISTORY = 0x22,
        /// <summary>
        /// The file system directory that contains application data for all users. A typical path is 
        /// C:\Documents and Settings\All Users\Application Data. This folder is used for application
        /// data that is not user specific. For example, an application can store a spell-check dictionary, 
        /// a database of clip art, or a log file in the CSIDL_COMMON_APPDATA folder. This information 
        /// will not roam and is available to anyone using the computer.
        /// </summary>
        CSIDL_COMMON_APPDATA = 0x23,
        /// <summary>
        /// The Windows directory or SYSROOT. This corresponds to the %windir% or %SYSTEMROOT% 
        /// environment variables. A typical path is C:\Windows.
        /// </summary>
        CSIDL_WINDOWS = 0x24,
        /// <summary>
        /// The Windows System folder. A typical path is C:\Windows\System32.
        /// </summary>
        CSIDL_SYSTEM = 0x25,
        /// <summary>
        /// The Program Files folder. A typical path is C:\Program Files.
        /// </summary>
        CSIDL_PROGRAM_FILES = 0x26,
        /// <summary>
        /// The file system directory that serves as a common repository for image files. 
        /// A typical path is C:\Documents and Settings\username\My Documents\My Pictures.
        /// </summary>
        CSIDL_MYPICTURES = 0x27,
        /// <summary>
        /// The user's profile folder. A typical path is C:\Users\username. Applications should not create 
        /// files or folders at this level; they should put their data under the locations referred to by 
        /// CSIDL_APPDATA or CSIDL_LOCAL_APPDATA. However, if you are creating a new Known Folder the profile 
        /// root referred to by CSIDL_PROFILE is appropriate.
        /// </summary>
        CSIDL_PROFILE = 0x28,
        /// <summary>
        /// The Windows System folder.
        /// </summary>
        CSIDL_SYSTEMX86 = 0x29,
        /// <summary>
        /// The x86 Program Files folder. A typical path is C:\Program Files (x86).
        /// </summary>
        CSIDL_PROGRAM_FILESX86 = 0x2A,
        /// <summary>
        /// A folder for components that are shared across applications. 
        /// A typical path is C:\Program Files\Common. Valid only for Windows XP.
        /// </summary>
        CSIDL_PROGRAM_FILES_COMMON = 0x2B,
        /// <summary>
        /// A folder for components that are shared across applications.
        /// </summary>
        CSIDL_PROGRAM_FILES_COMMONX86 = 0x2C,
        /// <summary>
        /// The file system directory that contains the templates that are available to all users. 
        /// A typical path is C:\Documents and Settings\All Users\Templates.
        /// </summary>
        CSIDL_COMMON_TEMPLATES = 0x2D,
        /// <summary>
        /// The file system directory that contains documents that are common to all users. 
        /// A typical path is C:\Documents and Settings\All Users\Documents.
        /// </summary>
        CSIDL_COMMON_DOCUMENTS = 0x2E,
        /// <summary>
        /// The file system directory that contains administrative tools for all users of the computer.
        /// </summary>
        CSIDL_COMMON_ADMINTOOLS = 0x2F,
        /// <summary>
        ///  The file system directory that is used to store administrative tools for an individual user. 
        ///  The MMC will save customized consoles to this directory, and it will roam with the user.
        /// </summary>
        CSIDL_ADMINTOOLS = 0x30,
        /// <summary>
        /// The virtual folder that represents Network Connections, that contains network and dial-up 
        /// connections.
        /// </summary>
        CSIDL_CONNECTIONS = 0x31,
        /// <summary>
        /// The file system directory that serves as a repository for music files common to all users. 
        /// A typical path is C:\Documents and Settings\All Users\Documents\My Music.
        /// </summary>
        CSIDL_COMMON_MUSIC = 0x35,
        /// <summary>
        /// The file system directory that serves as a repository for image files common to all users. 
        /// A typical path is C:\Documents and Settings\All Users\Documents\My Pictures.
        /// </summary>
        CSIDL_COMMON_PICTURES = 0x36,
        /// <summary>
        /// The file system directory that serves as a repository for video files common to all users.
        /// A typical path is C:\Documents and Settings\All Users\Documents\My Videos.
        /// </summary>
        CSIDL_COMMON_VIDEO = 0x37,
        /// <summary>
        /// Windows Vista. The file system directory that contains resource data. 
        /// A typical path is C:\Windows\Resources.
        /// </summary>
        CSIDL_RESOURCES = 0x38,
        /// <summary>
        /// Windows Vista. The file system directory that contains localized resource data.
        /// </summary>
        CSIDL_RESOURCES_LOCALIZED = 0x39,
        /// <summary>
        /// This value is recognized in Windows Vista for backward compatibility, but the folder itself is 
        /// no longer used.
        /// </summary>
        CSIDL_COMMON_OEM_LINKS = 0x3A,
        /// <summary>
        /// The file system directory that acts as a staging area for files waiting to be written to a CD. 
        /// A typical path is C:\Documents and Settings\username\Local Settings\Application Data\Microsoft\CD Burning.
        /// </summary>
        CSIDL_CDBURN_AREA = 0x3B,
        /// <summary>
        /// The folder that represents other computers in your workgroup.
        /// </summary>
        CSIDL_COMPUTERSNEARME = 0x3D,
        /// <summary>
        /// Combine with another CSIDL to force the creation of the associated folder if it does not exist.
        /// </summary>
        CSIDL_FLAG_CREATE = 0x8000,
        /// <summary>
        /// Combine with another CSIDL constant, except for CSIDL_FLAG_CREATE, to return an unverified folder path
        /// with no attempt to create or initialize the folder.
        /// </summary>
        CSIDL_FLAG_DONT_VERIFY = 0x4000,
        /// <summary>
        /// Combine with another CSIDL constant to ensure the expansion of environment variables.
        /// </summary>
        CSIDL_FLAG_DONT_UNEXPAND = 0x2000,
        /// <summary>
        /// Combine with another CSIDL constant to ensure the retrieval of the true system path for the folder, 
        /// free of any aliased placeholders such as %USERPROFILE%, returned by SHGetFolderLocation. 
        /// This flag has no effect on paths returned by SHGetFolderPath.
        /// </summary>
        CSIDL_FLAG_NO_ALIAS = 0x1000,
        /// <summary>
        /// combine with CSIDL_ value to indicate per-user init (eg. upgrade)
        /// </summary>
        CSIDL_FLAG_PER_USER_INIT = 0x8000,
        /// <summary>
        /// A mask for any valid CSIDL flag value.
        /// </summary>
        CSIDL_FLAG_MASK = 0xFF00
    }
}
