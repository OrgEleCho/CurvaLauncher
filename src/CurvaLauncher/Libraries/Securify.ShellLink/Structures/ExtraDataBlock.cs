using System;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// EXTRA_DATA_BLOCK: A structure consisting of any one of the following property data blocks.
    /// 
    /// CONSOLE_PROPS: A ConsoleDataBlock structure
    /// CONSOLE_FE_PROPS: A ConsoleFEDataBlock structure
    /// DARWIN_PROPS: A DarwinDataBlock structure
    /// ENVIRONMENT_PROPS:An EnvironmentVariableDataBlock structure
    /// ICON_ENVIRONMENT_PROPS: An IconEnvironmentDataBlock structure
    /// KNOWN_FOLDER_PROPS: A KnownFolderDataBlock structure
    /// PROPERTY_STORE_PROPS: A PropertyStoreDataBlock structure
    /// SHIM_PROPS: A ShimDataBlock structure
    /// SPECIAL_FOLDER_PROPS: A SpecialFolderDataBlock structure
    /// TRACKER_PROPS: A TrackerDataBlock structure
    /// VISTA_AND_ABOVE_IDLIST_PROPS: A VistaAndAboveIDListDataBlock structure
    /// </summary>
    public abstract class ExtraDataBlock : Structure
    {
        /// <summary>
        /// BlockSize (4 bytes): A 32-bit, unsigned integer that specifies the size of the ExtraDataBlock structure.
        /// </summary>
        public abstract uint BlockSize { get; }

        /// <summary>
        /// BlockSignature (4 bytes): A 32-bit, unsigned integer that specifies the signature of the ExtraDataBlock extra data section.
        /// </summary>
        public abstract BlockSignature BlockSignature { get; }

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("BlockSize: {0} (0x{0:X})", BlockSize);
            builder.AppendLine();
            builder.AppendFormat("BlockSignature: 0x{0:X}", BlockSignature);
            builder.AppendLine();
            return builder.ToString();
        }
        #endregion // ToString
    }

    #region BlockSignature
    /// <summary>
    /// BlockSignature (4 bytes): A 32-bit, unsigned integer that specifies the signature of the ExtraDataBlock extra data section.
    /// </summary>
    public enum BlockSignature : uint
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        ENVIRONMENT_PROPS = 0xA0000001,
        CONSOLE_PROPS = 0xA0000002,
        TRACKER_PROPS = 0xA0000003,
        CONSOLE_FE_PROPS = 0xA0000004,
        SPECIAL_FOLDER_PROPS = 0xA0000005,
        DARWIN_PROPS = 0xA0000006,
        ICON_ENVIRONMENT_PROPS = 0xA0000007,
        SHIM_PROPS = 0xA0000008,
        PROPERTY_STORE_PROPS = 0xA0000009,
        KNOWN_FOLDER_PROPS = 0xA000000B,
        VISTA_AND_ABOVE_IDLIST_PROPS = 0xA000000C
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
    #endregion // BlockSignature
}
