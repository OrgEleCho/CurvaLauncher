using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// DriveType (4 bytes): A 32-bit, unsigned integer that specifies the type of drive the link target is stored on.
    /// </summary>
    public enum DriveType : uint
    {
        /// <summary>
        /// The drive type cannot be determined.
        /// </summary>
        DRIVE_UNKNOWN = 0x00000000,
        /// <summary>
        /// The root path is invalid; for example, there is no volume mounted at the path.
        /// </summary>
        DRIVE_NO_ROOT_DIR = 0x00000001,
        /// <summary>
        /// The drive has removable media, such as a floppy drive, thumb drive, or flash card reader.
        /// </summary>
        DRIVE_REMOVABLE = 0x00000002,
        /// <summary>
        /// The drive has fixed media, such as a hard drive or flash drive.
        /// </summary>
        DRIVE_FIXED = 0x00000003,
        /// <summary>
        /// The drive is a remote (network) drive.
        /// </summary>
        DRIVE_REMOTE = 0x00000004,
        /// <summary>
        /// The drive is a CD-ROM drive.
        /// </summary>
        DRIVE_CDROM = 0x00000005,
        /// <summary>
        /// The drive is a RAM disk.
        /// </summary>
        DRIVE_RAMDISK = 0x00000006,
    }
}
