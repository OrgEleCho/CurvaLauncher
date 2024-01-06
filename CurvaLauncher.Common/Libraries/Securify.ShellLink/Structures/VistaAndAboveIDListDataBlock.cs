using System;
using System.Linq;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The VistaAndAboveIDListDataBlock structure specifies an alternate IDList that can 
    /// be used instead of the LinkTargetIDList structure (section 2.2) on platforms that 
    /// support it
    /// </summary>
    public class VistaAndAboveIDListDataBlock : ExtraDataBlock
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public VistaAndAboveIDListDataBlock() : base()
        {
            IDList = new IDList();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IDList">An IDList structure</param>
        public VistaAndAboveIDListDataBlock(IDList IDList) : base()
        {
            this.IDList = IDList;
        }
        #endregion // Constructor

        /// <summary>
        /// BlockSize (4 bytes): A 32-bit, unsigned integer that specifies the size of the 
        /// VistaAndAboveIDListDataBlock structure. This value MUST be greater than or equal 
        /// to 0x0000000A.
        /// </summary>
        public override uint BlockSize => 8 + (uint)IDList.IDListSize;

        /// <summary>
        /// BlockSignature (4 bytes): A 32-bit, unsigned integer that specifies the signature of 
        /// the VistaAndAboveIDListDataBlock extra data section. This value MUST be 0xA000000C.
        /// </summary>
        public override BlockSignature BlockSignature => BlockSignature.VISTA_AND_ABOVE_IDLIST_PROPS;

        /// <summary>
        /// IDList (variable): An IDList structure.
        /// </summary>
        public IDList IDList { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] VistaAndAboveIDListDataBlock = new byte[BlockSize];
            Buffer.BlockCopy(BitConverter.GetBytes(BlockSize), 0, VistaAndAboveIDListDataBlock, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)BlockSignature), 0, VistaAndAboveIDListDataBlock, 4, 4);
            Buffer.BlockCopy(IDList.GetBytes(), 0, VistaAndAboveIDListDataBlock, 8, (int)BlockSize - 8);
            return VistaAndAboveIDListDataBlock;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.Append(IDList.ToString());
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a VistaAndAboveIDListDataBlock from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A VistaAndAboveIDListDataBlock object</returns>
        public static VistaAndAboveIDListDataBlock FromByteArray(byte[] ba)
        {
            VistaAndAboveIDListDataBlock VistaAndAboveIDListDataBlock = new VistaAndAboveIDListDataBlock();
            if (ba.Length < 0xA)
            {
                throw new ArgumentException(string.Format("Size of the VistaAndAboveIDListDataBlock Structure is less than 10 ({0})", ba.Length));
            }

            uint BlockSize = BitConverter.ToUInt32(ba, 0);
            if (BlockSize > ba.Length)
            {
                throw new ArgumentException(string.Format("BlockSize is {0} is incorrect (expected {1})", BlockSize, VistaAndAboveIDListDataBlock.BlockSize));
            }

            BlockSignature BlockSignature = (BlockSignature)BitConverter.ToUInt32(ba, 4);
            if (BlockSignature != VistaAndAboveIDListDataBlock.BlockSignature)
            {
                throw new ArgumentException(string.Format("BlockSignature is {0} is incorrect (expected {1})", BlockSignature, VistaAndAboveIDListDataBlock.BlockSignature));
            }

            ba = ba.Skip(8).ToArray();
            uint Count = BlockSize - 8;
            while (Count > 0)
            {
                ushort ItemIDSize = BitConverter.ToUInt16(ba, 0);
                if (ItemIDSize != 0)
                {
                    byte[] itemID = new byte[ItemIDSize - 2];
                    Buffer.BlockCopy(ba, 2, itemID, 0, itemID.Length);
                    Count -= ItemIDSize;
                    VistaAndAboveIDListDataBlock.IDList.ItemIDList.Add(new ItemID(itemID));
                    ba = ba.Skip(ItemIDSize).ToArray();
                }
                else
                {
                    break;
                }
            }

            return VistaAndAboveIDListDataBlock;
        }
        #endregion // FromByteArray
    }
}
