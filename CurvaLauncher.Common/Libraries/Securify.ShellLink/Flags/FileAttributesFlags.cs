using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// The FileAttributesFlags structure defines bits that specify the file attributes of the 
    /// link target, if the target is a file system item. File attributes can be used if the 
    /// link target is not available, or if accessing the target would be inefficient. It is 
    /// possible for the target items attributes to be out of sync with this value.
    /// </summary>
    [Flags]
    public enum FileAttributesFlags : uint
    {
        /// <summary>
        /// The file or directory is read-only. For a file, if this bit is set, applications can 
        /// read the file but cannot write to it or delete it. For a directory, if this bit is set, 
        /// applications cannot delete the directory.
        /// </summary>
        FILE_ATTRIBUTE_READONLY = 0x1,
        /// <summary>
        /// The file or directory is hidden. If this bit is set, the file or folder is not included 
        /// in an ordinary directory listing.
        /// </summary>
        FILE_ATTRIBUTE_HIDDEN = 0x2,
        /// <summary>
        /// The file or directory is part of the operating system or is used exclusively by the 
        /// operating system.
        /// </summary>
        FILE_ATTRIBUTE_SYSTEM = 0x4,
        /// <summary>
        /// A bit that MUST be zero.
        /// </summary>
        Reserved1 = 0x8,
        /// <summary>
        /// The link target is a directory instead of a file.
        /// </summary>
        FILE_ATTRIBUTE_DIRECTORY = 0x10,
        /// <summary>
        /// The file or directory is an archive file. Applications use this flag to mark files for 
        /// backup or removal.
        /// </summary>
        FILE_ATTRIBUTE_ARCHIVE = 0x20,
        /// <summary>
        /// A bit that MUST be zero.
        /// </summary>
        Reserved2 = 0x40,
        /// <summary>
        /// The file or directory has no other flags set. If this bit is 1, all other bits in this 
        /// structure MUST be clear.
        /// </summary>
        FILE_ATTRIBUTE_NORMAL = 0x80,
        /// <summary>
        /// The file is being used for temporary storage.
        /// </summary>
        FILE_ATTRIBUTE_TEMPORARY = 0x100,
        /// <summary>
        /// The file is a sparse file.
        /// </summary>
        FILE_ATTRIBUTE_SPARSE_FILE = 0x200,
        /// <summary>
        /// The file or directory has an associated reparse point.
        /// </summary>
        FILE_ATTRIBUTE_REPARSE_POINT = 0x400,
        /// <summary>
        /// The file or directory is compressed. For a file, this means that all data in the file is 
        /// compressed. For a directory, this means that compression is the default for newly created 
        /// files and subdirectories.
        /// </summary>
        FILE_ATTRIBUTE_COMPRESSED = 0x800,
        /// <summary>
        /// The data of the file is not immediately available.
        /// </summary>
        FILE_ATTRIBUTE_OFFLINE = 0x1000,
        /// <summary>
        /// The contents of the file need to be indexed.
        /// </summary>
        FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x2000,
        /// <summary>
        /// The file or directory is encrypted. For a file, this means that all data in the file is 
        /// encrypted. For a directory, this means that encryption is the default for newly created 
        /// files and subdirectories.
        /// </summary>
        FILE_ATTRIBUTE_ENCRYPTED = 0x4000
    }
}
