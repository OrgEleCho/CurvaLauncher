using System;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The DarwinDataBlock structure specifies an application identifier that can be used 
    /// instead of a link target IDList to install an application when a shell link is activated.
    /// </summary>
    public class DarwinDataBlock : ExtraDataBlock
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DarwinDataBlock() : base()
        {
            DarwinDataAnsi = "";
            DarwinDataUnicode = "";
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="DarwinData">A NULL–terminated string, which specifies an application identifier.</param>
        public DarwinDataBlock(string DarwinData) : base()
        {
            DarwinDataAnsi = DarwinData;
            DarwinDataUnicode = DarwinData;
        }
        #endregion // Constructor

        /// <summary>
        /// BlockSize (4 bytes): A 32-bit, unsigned integer that specifies the size of the 
        /// DarwinDataBlock structure. This value MUST be 0x00000314.
        /// </summary>
        public override uint BlockSize => 0x314;

        /// <summary>
        /// BlockSignature (4 bytes): A 32-bit, unsigned integer that specifies the signature 
        /// of the DarwinDataBlock extra data section. This value MUST be 0xA0000006.
        /// </summary>
        public override BlockSignature BlockSignature => BlockSignature.DARWIN_PROPS;

        /// <summary>
        /// DarwinDataAnsi (260 bytes): A NULL–terminated string, defined by the system default 
        /// code page, which specifies an application identifier. This field SHOULD be ignored.
        /// </summary>
        public string DarwinDataAnsi { get; set; }

        /// <summary>
        /// DarwinDataUnicode (520 bytes): An optional, NULL–terminated, Unicode string that 
        /// specifies an application identifier.
        /// </summary>
        public string DarwinDataUnicode { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] EarwinDataBlock = new byte[BlockSize];
            Buffer.BlockCopy(BitConverter.GetBytes(BlockSize), 0, EarwinDataBlock, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)BlockSignature), 0, EarwinDataBlock, 4, 4);
            Buffer.BlockCopy(Encoding.Default.GetBytes(DarwinDataAnsi), 0, EarwinDataBlock, 8, DarwinDataAnsi.Length < 259 ? DarwinDataAnsi.Length : 259);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(DarwinDataUnicode), 0, EarwinDataBlock, 268, DarwinDataUnicode.Length < 259 ? DarwinDataUnicode.Length * 2 : 518);
            return EarwinDataBlock;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("DarwinDataAnsi: {0}", DarwinDataAnsi);
            builder.AppendLine();
            builder.AppendFormat("DarwinDataUnicode: {0}", DarwinDataUnicode);
            builder.AppendLine();
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a ShellLinkHeader from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A ShellLinkHeader object</returns>
        public static DarwinDataBlock FromByteArray(byte[] ba)
        {
            DarwinDataBlock DarwinDataBlock = new DarwinDataBlock();
            if (ba.Length < 0x314)
            {
                throw new ArgumentException(string.Format("Size of the DarwinDataBlock Structure is less than 788 ({0})", ba.Length));
            }

            uint BlockSize = BitConverter.ToUInt32(ba, 0);
            if (BlockSize > ba.Length)
            {
                throw new ArgumentException(string.Format("BlockSize is {0} is incorrect (expected {1})", BlockSize, DarwinDataBlock.BlockSize));
            }

            BlockSignature BlockSignature = (BlockSignature)BitConverter.ToUInt32(ba, 4);
            if (BlockSignature != DarwinDataBlock.BlockSignature)
            {
                throw new ArgumentException(string.Format("BlockSignature is {0} is incorrect (expected {1})", BlockSignature, DarwinDataBlock.BlockSignature));
            }

            byte[] DarwinDataAnsi = new byte[260];
            Buffer.BlockCopy(ba, 8, DarwinDataAnsi, 0, 260);
            DarwinDataBlock.DarwinDataAnsi = Encoding.Default.GetString(DarwinDataAnsi).TrimEnd(new char[] { (char)0 });

            byte[] DarwinDataUnicode = new byte[520];
            Buffer.BlockCopy(ba, 268, DarwinDataUnicode, 0, 520);
            DarwinDataBlock.DarwinDataUnicode = Encoding.Unicode.GetString(DarwinDataUnicode).TrimEnd(new char[] { (char)0 });

            return DarwinDataBlock;
        }
        #endregion // FromByteArray
    }
}
