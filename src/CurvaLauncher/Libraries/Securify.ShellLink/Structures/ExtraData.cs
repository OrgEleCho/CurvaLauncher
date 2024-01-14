using System;
using System.Linq;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// ExtraData refers to a set of structures that convey additional information about a link target. 
    /// These optional structures can be present in an extra data section that is appended to the basic 
    /// Shell Link Binary File Format.
    /// </summary>
    public class ExtraData : Structure
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ExtraData() : base() { TerminalBlock = new TerminalBlock(); }
        #endregion // Constructor

        #region ExtraDataSize
        /// <summary>
        /// Total size in bytes of the ExtraData structure
        /// </summary>
        public int ExtraDataSize
        {
            get
            {
                uint Size = 4;
                Size += ConsoleDataBlock != null ? ConsoleDataBlock.BlockSize : 0;
                Size += ConsoleFEDataBlock != null ? ConsoleFEDataBlock.BlockSize : 0;
                Size += DarwinDataBlock != null ? DarwinDataBlock.BlockSize : 0;
                Size += EnvironmentVariableDataBlock != null ? EnvironmentVariableDataBlock.BlockSize : 0;
                Size += IconEnvironmentDataBlock != null ? IconEnvironmentDataBlock.BlockSize : 0;
                Size += KnownFolderDataBlock != null ? KnownFolderDataBlock.BlockSize : 0;
                Size += PropertyStoreDataBlock != null ? PropertyStoreDataBlock.BlockSize : 0;
                Size += ShimDataBlock != null ? ShimDataBlock.BlockSize : 0;
                Size += SpecialFolderDataBlock != null ? SpecialFolderDataBlock.BlockSize : 0;
                Size += TrackerDataBlock != null ? TrackerDataBlock.BlockSize : 0;
                Size += VistaAndAboveIDListDataBlock != null ? VistaAndAboveIDListDataBlock.BlockSize : 0;
                return (int)Size;
            }
        }
        #endregion // ExtraDataSize

        /// <summary>
        /// CONSOLE_PROPS: A ConsoleDataBlock structure
        /// </summary>
        public ConsoleDataBlock? ConsoleDataBlock { get; set; }

        /// <summary>
        /// CONSOLE_FE_PROPS: A ConsoleFEDataBlock structure
        /// </summary>
        public ConsoleFEDataBlock? ConsoleFEDataBlock { get; set; }

        /// <summary>
        /// DARWIN_PROPS: A DarwinDataBlock structure
        /// </summary>
        public DarwinDataBlock? DarwinDataBlock { get; set; }

        /// <summary>
        /// ENVIRONMENT_PROPS:An EnvironmentVariableDataBlock structure
        /// </summary>
        public EnvironmentVariableDataBlock? EnvironmentVariableDataBlock { get; set; }

        /// <summary>
        /// ICON_ENVIRONMENT_PROPS: An IconEnvironmentDataBlock structure
        /// </summary>
        public IconEnvironmentDataBlock? IconEnvironmentDataBlock { get; set; }

        /// <summary>
        /// KNOWN_FOLDER_PROPS: A KnownFolderDataBlock structure
        /// </summary>
        public KnownFolderDataBlock? KnownFolderDataBlock { get; set; }

        /// <summary>
        /// PROPERTY_STORE_PROPS: A PropertyStoreDataBlock structure
        /// </summary>
        public PropertyStoreDataBlock? PropertyStoreDataBlock { get; set; }

        /// <summary>
        /// SHIM_PROPS: A ShimDataBlock structure
        /// </summary>
        public ShimDataBlock? ShimDataBlock { get; set; }

        /// <summary>
        /// SPECIAL_FOLDER_PROPS: A SpecialFolderDataBlock structure
        /// </summary>
        public SpecialFolderDataBlock? SpecialFolderDataBlock { get; set; }

        /// <summary>
        /// TRACKER_PROPS: A TrackerDataBlock structure
        /// </summary>
        public TrackerDataBlock? TrackerDataBlock { get; set; }

        /// <summary>
        /// VISTA_AND_ABOVE_IDLIST_PROPS: A VistaAndAboveIDListDataBlock structure
        /// </summary>
        public VistaAndAboveIDListDataBlock? VistaAndAboveIDListDataBlock { get; set; }

        /// <summary>
        /// TERMINAL_BLOCK: A structure that indicates the end of the extra data section.
        /// </summary>
        public TerminalBlock TerminalBlock { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            int Offset = 0;
            byte[] ExtraData = new byte[ExtraDataSize];

            if (ConsoleDataBlock != null)
            {
                Buffer.BlockCopy(ConsoleDataBlock.GetBytes(), 0, ExtraData, Offset, (int)ConsoleDataBlock.BlockSize);
                Offset += (int)ConsoleDataBlock.BlockSize;
            }

            if (ConsoleFEDataBlock != null)
            {
                Buffer.BlockCopy(ConsoleFEDataBlock.GetBytes(), 0, ExtraData, Offset, (int)ConsoleFEDataBlock.BlockSize);
                Offset += (int)ConsoleFEDataBlock.BlockSize;
            }

            if (DarwinDataBlock != null)
            {
                Buffer.BlockCopy(DarwinDataBlock.GetBytes(), 0, ExtraData, Offset, (int)DarwinDataBlock.BlockSize);
                Offset += (int)DarwinDataBlock.BlockSize;
            }

            if (EnvironmentVariableDataBlock != null)
            {
                Buffer.BlockCopy(EnvironmentVariableDataBlock.GetBytes(), 0, ExtraData, Offset, (int)EnvironmentVariableDataBlock.BlockSize);
                Offset += (int)EnvironmentVariableDataBlock.BlockSize;
            }

            if (IconEnvironmentDataBlock != null)
            {
                Buffer.BlockCopy(IconEnvironmentDataBlock.GetBytes(), 0, ExtraData, Offset, (int)IconEnvironmentDataBlock.BlockSize);
                Offset += (int)IconEnvironmentDataBlock.BlockSize;
            }

            if (KnownFolderDataBlock != null)
            {
                Buffer.BlockCopy(KnownFolderDataBlock.GetBytes(), 0, ExtraData, Offset, (int)KnownFolderDataBlock.BlockSize);
                Offset += (int)KnownFolderDataBlock.BlockSize;
            }

            if (PropertyStoreDataBlock != null)
            {
                Buffer.BlockCopy(PropertyStoreDataBlock.GetBytes(), 0, ExtraData, Offset, (int)PropertyStoreDataBlock.BlockSize);
                Offset += (int)PropertyStoreDataBlock.BlockSize;
            }

            if (ShimDataBlock != null)
            {
                Buffer.BlockCopy(ShimDataBlock.GetBytes(), 0, ExtraData, Offset, (int)ShimDataBlock.BlockSize);
                Offset += (int)ShimDataBlock.BlockSize;
            }

            if (SpecialFolderDataBlock != null)
            {
                Buffer.BlockCopy(SpecialFolderDataBlock.GetBytes(), 0, ExtraData, Offset, (int)SpecialFolderDataBlock.BlockSize);
                Offset += (int)SpecialFolderDataBlock.BlockSize;
            }

            if (TrackerDataBlock != null)
            {
                Buffer.BlockCopy(TrackerDataBlock.GetBytes(), 0, ExtraData, Offset, (int)TrackerDataBlock.BlockSize);
                Offset += (int)TrackerDataBlock.BlockSize;
            }

            if (VistaAndAboveIDListDataBlock != null)
            {
                Buffer.BlockCopy(VistaAndAboveIDListDataBlock.GetBytes(), 0, ExtraData, Offset, (int)VistaAndAboveIDListDataBlock.BlockSize);
                Offset += (int)VistaAndAboveIDListDataBlock.BlockSize;
            }

            Buffer.BlockCopy(TerminalBlock.GetBytes(), 0, ExtraData, Offset, 4);
            return ExtraData;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());

            if (ConsoleDataBlock != null)
            {
                builder.Append(ConsoleDataBlock.ToString());
            }

            if (ConsoleFEDataBlock != null)
            {
                builder.Append(ConsoleFEDataBlock.ToString());
            }

            if (DarwinDataBlock != null)
            {
                builder.Append(DarwinDataBlock.ToString());
            }

            if (EnvironmentVariableDataBlock != null)
            {
                builder.Append(EnvironmentVariableDataBlock.ToString());
            }

            if (IconEnvironmentDataBlock != null)
            {
                builder.Append(IconEnvironmentDataBlock.ToString());
            }

            if (KnownFolderDataBlock != null)
            {
                builder.Append(KnownFolderDataBlock.ToString());
            }

            if (PropertyStoreDataBlock != null)
            {
                builder.Append(PropertyStoreDataBlock.ToString());
            }

            if (ShimDataBlock != null)
            {
                builder.Append(ShimDataBlock.ToString());
            }

            if (SpecialFolderDataBlock != null)
            {
                builder.Append(SpecialFolderDataBlock.ToString());
            }

            if (TrackerDataBlock != null)
            {
                builder.Append(TrackerDataBlock.ToString());
            }

            if (VistaAndAboveIDListDataBlock != null)
            {
                builder.Append(VistaAndAboveIDListDataBlock.ToString());
            }

            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create an ExtraData from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>An ExtraData object</returns>
        public static ExtraData FromByteArray(byte[] ba)
        {
            ExtraData ExtraData = new ExtraData();
            if (ba.Length < 4)
            {
                throw new ArgumentException(string.Format("Size of the ExtraData Structure is less than 4 ({0})", ba.Length));
            }

            uint BlockSize = BitConverter.ToUInt32(ba, 0);

            while (BlockSize > 4)
            {
                if (BlockSize > ba.Length)
                {
                    throw new ArgumentException(string.Format("BlockSize is {0} is incorrect (bytes left {1})", BlockSize, ba.Length));
                }
                BlockSignature BlockSignature = (BlockSignature)BitConverter.ToUInt32(ba, 4);
                switch (BlockSignature)
                {
                    case BlockSignature.CONSOLE_PROPS:
                        ExtraData.ConsoleDataBlock = ConsoleDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.CONSOLE_FE_PROPS:
                        ExtraData.ConsoleFEDataBlock = ConsoleFEDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.DARWIN_PROPS:
                        ExtraData.DarwinDataBlock = DarwinDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.ENVIRONMENT_PROPS:
                        ExtraData.EnvironmentVariableDataBlock = EnvironmentVariableDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.ICON_ENVIRONMENT_PROPS:
                        ExtraData.IconEnvironmentDataBlock = IconEnvironmentDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.KNOWN_FOLDER_PROPS:
                        ExtraData.KnownFolderDataBlock = KnownFolderDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.PROPERTY_STORE_PROPS:
                        ExtraData.PropertyStoreDataBlock = PropertyStoreDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.SHIM_PROPS:
                        ExtraData.ShimDataBlock = ShimDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.SPECIAL_FOLDER_PROPS:
                        ExtraData.SpecialFolderDataBlock = SpecialFolderDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.TRACKER_PROPS:
                        ExtraData.TrackerDataBlock = TrackerDataBlock.FromByteArray(ba);
                        break;
                    case BlockSignature.VISTA_AND_ABOVE_IDLIST_PROPS:
                        ExtraData.VistaAndAboveIDListDataBlock = VistaAndAboveIDListDataBlock.FromByteArray(ba);
                        break;
                    default:
                        throw new ArgumentException(string.Format("BlockSignature is {0} is incorrect", BlockSignature));
                }
                ba = ba.Skip((int)BlockSize).ToArray();
                BlockSize = BitConverter.ToUInt32(ba, 0);
            }

            return ExtraData;
        }
        #endregion // FromByteArray
    }
}
