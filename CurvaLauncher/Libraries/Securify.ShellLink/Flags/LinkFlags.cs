using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// The LinkFlags structure defines bits that specify which shell link structures are 
    /// present in the file format after the ShellLinkHeader structure
    /// </summary>
    [Flags]
    public enum LinkFlags : uint
    {
        /// <summary>
        /// The shell link is saved with an item ID list (IDList). If this bit is set, a 
        /// LinkTargetIDList structure MUST follow the ShellLinkHeader. If this bit is not 
        /// set, this structure MUST NOT be present.
        /// </summary>
        HasLinkTargetIDList = 0x1,
        /// <summary>
        /// The shell link is saved with link information. If this bit is set, a LinkInfo 
        /// structure MUST be present. If this bit is not set, this structure MUST NOT be present.
        /// </summary>
        HasLinkInfo = 0x2,
        /// <summary>
        /// The shell link is saved with a name string. If this bit is set, a NAME_STRING StringData 
        /// structure MUST be present. If this bit is not set, this structure MUST NOT be present.
        /// </summary>
        HasName = 0x4,
        /// <summary>
        /// The shell link is saved with a relative path string. If this bit is set, a RELATIVE_PATH 
        /// StringData structure MUST be present. If this bit is not set, this structure MUST NOT be 
        /// present.
        /// </summary>
        HasRelativePath = 0x8,
        /// <summary>
        /// The shell link is saved with a working directory string. If this bit is set, a 
        /// WORKING_DIR StringData structure MUST be present. If this bit is not set, this structure 
        /// MUST NOT be present.
        /// </summary>
        HasWorkingDir = 0x10,
        /// <summary>
        /// The shell link is saved with command line arguments. If this bit is set, a 
        /// COMMAND_LINE_ARGUMENTS StringData structure MUST be present. If this bit is not set, 
        /// this structure MUST NOT be present.
        /// </summary>
        HasArguments = 0x20,
        /// <summary>
        /// The shell link is saved with an icon location string. If this bit is set, an ICON_LOCATION 
        /// StringData structure MUST be present. If this bit is not set, this structure MUST NOT be present.
        /// </summary>
        HasIconLocation = 0x40,
        /// <summary>
        /// The shell link contains Unicode encoded strings. This bit SHOULD be set. If this bit is set, 
        /// the StringData section contains Unicode-encoded strings; otherwise, it contains strings that are 
        /// encoded using the system default code page.
        /// </summary>
        IsUnicode = 0x80,
        /// <summary>
        /// The LinkInfo structure is ignored.
        /// </summary>
        ForceNoLinkInfo = 0x100,
        /// <summary>
        /// The shell link is saved with an EnvironmentVariableDataBlock.
        /// </summary>
        HasExpString = 0x200,
        /// <summary>
        /// The target is run in a separate virtual machine when launching a link target that is a 16-bit 
        /// application.
        /// </summary>
        RunInSeparateProcess = 0x400,
        /// <summary>
        /// A bit that is undefined and MUST be ignored.
        /// </summary>
        Unused1 = 0x800,
        /// <summary>
        /// The shell link is saved with a DarwinDataBlock.
        /// </summary>
        HasDarwinID = 0x1000,
        /// <summary>
        /// The application is run as a different user when the target of the shell link is activated.
        /// </summary>
        RunAsUser = 0x2000,
        /// <summary>
        /// The shell link is saved with an IconEnvironmentDataBlock
        /// </summary>
        HasExpIcon = 0x4000,
        /// <summary>
        /// The file system location is represented in the shell namespace when the path to an item is 
        /// parsed into an IDList.
        /// </summary>
        NoPidlAlias = 0x8000,
        /// <summary>
        /// A bit that is undefined and MUST be ignored.
        /// </summary>
        Unused2 = 0x10000,
        /// <summary>
        /// The shell link is saved with a ShimDataBlock
        /// </summary>
        RunWithShimLayer = 0x20000,
        /// <summary>
        /// The TrackerDataBlock (section 2.5.10) is ignored.
        /// </summary>
        ForceNoLinkTrack = 0x40000,
        /// <summary>
        /// The shell link attempts to collect target properties and store them in the PropertyStoreDataBlock 
        /// when the link target is set.
        /// </summary>
        EnableTargetMetadata = 0x80000,
        /// <summary>
        /// The EnvironmentVariableDataBlock is ignored.
        /// </summary>
        DisableLinkPathTracking = 0x100000,
        /// <summary>
        /// The SpecialFolderDataBlock  and the KnownFolderDataBlock are ignored when loading the shell link. 
        /// If this bit is set, these extra data blocks SHOULD NOT be saved when saving the shell link.
        /// </summary>
        DisableKnownFolderTracking = 0x200000,
        /// <summary>
        /// If the link has a KnownFolderDataBlock, the unaliased form of the known folder IDList SHOULD be used 
        /// when translating the target IDList at the time that the link is loaded.
        /// </summary>
        DisableKnownFolderAlias = 0x400000,
        /// <summary>
        /// Creating a link that references another link is enabled. Otherwise, specifying a link as the target 
        /// IDList SHOULD NOT be allowed.
        /// </summary>
        AllowLinkToLink = 0x800000,
        /// <summary>
        /// When saving a link for which the target IDList is under a known folder, either the unaliased form of 
        /// that known folder or the target IDList SHOULD be used.
        /// </summary>
        UnaliasOnSave = 0x1000000,
        /// <summary>
        /// The target IDList SHOULD NOT be stored; instead, the path specified in the EnvironmentVariableDataBlock 
        /// SHOULD be used to refer to the target.
        /// </summary>
        PreferEnvironmentPath = 0x2000000,
        /// <summary>
        /// When the target is a UNC name that refers to a location on a local machine, the local path IDList in the
        /// PropertyStoreDataBlock SHOULD be stored, so it can be used when the link is loaded on the local machine.
        /// </summary>
        KeepLocalIDListForUNCTarget = 0x4000000
    }
}
