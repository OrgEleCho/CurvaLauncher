using System;
using System.Text;
using CurvaLauncher.Libraries.Securify.ShellLink.Flags;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The SpecialFolderDataBlock structure specifies the location of a special folder. This data can be 
    /// used when a link target is a special folder to keep track of the folder, so that the link target 
    /// IDList can be translated when the link is loaded.
    /// </summary>
    public class SpecialFolderDataBlock : ExtraDataBlock
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SpecialFolderDataBlock() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="SpecialFolderID">nsigned integer that specifies the folder integer ID.</param>
        /// <param name="Offset">The offset, in bytes, into the link target IDList.</param>
        public SpecialFolderDataBlock(CSIDL SpecialFolderID, uint Offset) : base()
        {
            this.SpecialFolderID = SpecialFolderID;
            this.Offset = Offset;
        }
        #endregion // Constructor

        /// <summary>
        /// BlockSize (4 bytes): A 32-bit, unsigned integer that specifies the size of the 
        /// SpecialFolderDataBlock structure. This value MUST be 0x00000010.
        /// </summary>
        public override uint BlockSize => 0x10;

        /// <summary>
        /// BlockSignature (4 bytes): A 32-bit, unsigned integer that specifies the signature 
        /// of the SpecialFolderDataBlock extra data section. This value MUST be 0xA0000005.
        /// </summary>
        public override BlockSignature BlockSignature => BlockSignature.SPECIAL_FOLDER_PROPS;

        /// <summary>
        /// SpecialFolderID (4 bytes): A 32-bit, unsigned integer that specifies the folder 
        /// integer ID.
        /// </summary>
        public CSIDL SpecialFolderID { get; set; }

        /// <summary>
        /// Offset (4 bytes): A 32-bit, unsigned integer that specifies the location of the 
        /// ItemID of the first child segment of the IDList specified by SpecialFolderID. 
        /// This value is the offset, in bytes, into the link target IDList.
        /// </summary>
        public uint Offset { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] SpecialFolderDataBlock = new byte[BlockSize];
            Buffer.BlockCopy(BitConverter.GetBytes(BlockSize), 0, SpecialFolderDataBlock, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)BlockSignature), 0, SpecialFolderDataBlock, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)SpecialFolderID), 0, SpecialFolderDataBlock, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Offset), 0, SpecialFolderDataBlock, 12, 4);
            return SpecialFolderDataBlock;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("SpecialFolderID: {0}", SpecialFolderID);
            builder.AppendLine();
            builder.AppendFormat("Offset: {0} (0x{0:X})", Offset);
            builder.AppendLine();
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a SpecialFolderDataBlock from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A SpecialFolderDataBlock object</returns>
        public static SpecialFolderDataBlock FromByteArray(byte[] ba)
        {
            SpecialFolderDataBlock SpecialFolderDataBlock = new SpecialFolderDataBlock();
            if (ba.Length < 0x10)
            {
                throw new ArgumentException(string.Format("Size of the SpecialFolderDataBlock Structure is less than 16 ({0})", ba.Length));
            }

            uint BlockSize = BitConverter.ToUInt32(ba, 0);
            if (BlockSize > ba.Length)
            {
                throw new ArgumentException(string.Format("BlockSize is {0} is incorrect (expected {1})", BlockSize, SpecialFolderDataBlock.BlockSize));
            }

            BlockSignature BlockSignature = (BlockSignature)BitConverter.ToUInt32(ba, 4);
            if (BlockSignature != SpecialFolderDataBlock.BlockSignature)
            {
                throw new ArgumentException(string.Format("BlockSignature is {0} is incorrect (expected {1})", BlockSignature, SpecialFolderDataBlock.BlockSignature));
            }

            SpecialFolderDataBlock.SpecialFolderID = (CSIDL)BitConverter.ToUInt32(ba, 8);
            SpecialFolderDataBlock.Offset = BitConverter.ToUInt32(ba, 12);

            return SpecialFolderDataBlock;
        }
        #endregion // FromByteArray
    }
}
