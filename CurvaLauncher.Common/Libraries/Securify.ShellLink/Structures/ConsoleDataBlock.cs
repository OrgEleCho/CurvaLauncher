using System;
using System.Text;
using CurvaLauncher.Libraries.Securify.ShellLink.Flags;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The ConsoleDataBlock structure specifies the display settings to use when a 
    /// link target specifies an application that is run in a console window
    /// </summary>
    public class ConsoleDataBlock : ExtraDataBlock
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ConsoleDataBlock() : base()
        {
            FaceName = "";
            ColorTable = new byte[64];
        }
        #endregion //Constructor

        /// <summary>
        /// BlockSize (4 bytes): A 32-bit, unsigned integer that specifies the size of the 
        /// ConsoleDataBlock structure. This value MUST be 0x000000CC
        /// </summary>
        public override uint BlockSize => 0xCC;

        /// <summary>
        /// BlockSignature (4 bytes): A 32-bit, unsigned integer that specifies the signature 
        /// of the ConsoleDataBlock extra data section. This value MUST be 0xA0000002.
        /// </summary>
        public override BlockSignature BlockSignature => BlockSignature.CONSOLE_PROPS;

        /// <summary>
        /// FillAttributes (2 bytes): A 16-bit, unsigned integer that specifies the fill 
        /// attributes that control the foreground and background text colors in the console window.
        /// </summary>
        public FillAttributes FillAttributes { get; set; }

        /// <summary>
        /// PopupFillAttributes (2 bytes): A 16-bit, unsigned integer that specifies the fill 
        /// attributes that control the foreground and background text color in the console window 
        /// popup. The values are the same as for the FillAttributes field.
        /// </summary>
        public FillAttributes PopupFillAttributes { get; set; }

        /// <summary>
        /// ScreenBufferSizeX (2 bytes): A 16-bit, signed integer that specifies the horizontal 
        /// size (X axis), in characters, of the console window buffer.
        /// </summary>
        public ushort ScreenBufferSizeX { get; set; }

        /// <summary>
        /// ScreenBufferSizeY (2 bytes): A 16-bit, signed integer that specifies the vertical 
        /// size (Y axis), in characters, of the console window buffer.
        /// </summary>
        public ushort ScreenBufferSizeY { get; set; }

        /// <summary>
        /// WindowSizeX (2 bytes): A 16-bit, signed integer that specifies the horizontal size 
        /// (X axis), in characters, of the console window.
        /// </summary>
        public ushort WindowSizeX { get; set; }

        /// <summary>
        /// WindowSizeY (2 bytes): A 16-bit, signed integer that specifies the vertical size 
        /// (Y axis), in characters, of the console window.
        /// </summary>
        public ushort WindowSizeY { get; set; }

        /// <summary>
        /// WindowOriginX (2 bytes): A 16-bit, signed integer that specifies the horizontal coordinate 
        /// (X axis), in pixels, of the console window origin.
        /// </summary>
        public ushort WindowOriginX { get; set; }

        /// <summary>
        /// WindowOriginY (2 bytes): A 16-bit, signed integer that specifies the vertical coordinate 
        /// (Y axis), in pixels, of the console window origin.
        /// </summary>
        public ushort WindowOriginY { get; set; }

        /// <summary>
        /// Unused1 (4 bytes): A value that is undefined and MUST be ignored.
        /// </summary>
        public uint Unused1 { get; set; }

        /// <summary>
        /// Unused2 (4 bytes): A value that is undefined and MUST be ignored.
        /// </summary>
        public uint Unused2 { get; set; }

        /// <summary>
        /// FontSize (4 bytes): A 32-bit, unsigned integer that specifies the size, in pixels, of the 
        /// font used in the console window.
        /// </summary>
        public uint FontSize { get; set; }

        /// <summary>
        /// FontFamily (4 bytes): A 32-bit, unsigned integer that specifies the family of the font used 
        /// in the console window.
        /// </summary>
        public FontFamily FontFamily { get; set; }
        /// <summary>
        /// A bitwise OR of one or more of the following font-pitch bits is added to the font family
        /// </summary>
        public FontPitch FontPitch { get; set; }

        /// <summary>
        /// FontWeight (4 bytes): A 16-bit, unsigned integer that specifies the stroke weight of the 
        /// font used in the console window.
        /// 
        /// 700 ≤ value     A bold font.
        /// value &lt; 700     A regular-weight font.
        /// </summary>
        public uint FontWeight { get; set; }

        /// <summary>
        /// Face Name (64 bytes): A 32-character Unicode string that specifies the face name of the 
        /// font used in the console window
        /// </summary>
        public string FaceName { get; set; }

        /// <summary>
        /// CursorSize (4 bytes): A 32-bit, unsigned integer that specifies the size of the cursor, 
        /// in pixels, used in the console window.
        /// 
        /// value ≤ 25      A small cursor.
        /// 26 — 50         A medium cursor.
        /// 51 — 100        A large cursor.
        /// </summary>
        public uint CursorSize { get; set; }

        /// <summary>
        /// FullScreen (4 bytes): A 32-bit, unsigned integer that specifies whether to open the console 
        /// window in full-screen mode.
        /// 
        /// 0x00000000          Full-screen mode is off.
        /// 0x00000000 &lt; value  Full-screen mode is on.
        /// </summary>
        public bool FullScreen { get; set; }

        /// <summary>
        /// QuickEdit (4 bytes): A 32-bit, unsigned integer that specifies whether to open the console 
        /// window in QuikEdit mode. In QuickEdit mode, the mouse can be used to cut, copy, and paste 
        /// text in the console window.
        /// 
        /// 0x00000000          QuickEdit mode is off.
        /// 0x00000000 &lt; value  QuickEdit mode is on.
        /// </summary>
        public bool QuickEdit { get; set; }

        /// <summary>
        /// InsertMode (4 bytes): A 32-bit, unsigned integer that specifies insert mode in the console 
        /// window.
        /// 
        /// 0x00000000          InsertMode mode is off.
        /// 0x00000000 &lt; value  InsertMode mode is on.
        /// </summary>
        public bool InsertMode { get; set; }

        /// <summary>
        /// AutoPosition (4 bytes): A 32-bit, unsigned integer that specifies auto-position mode of 
        /// the console window.
        /// 
        /// 0x00000000          AutoPosition mode is off.
        /// 0x00000000 &lt; value  AutoPosition mode is on.
        /// </summary>
        public bool AutoPosition { get; set; }

        /// <summary>
        /// HistoryBufferSize (4 bytes): A 32-bit, unsigned integer that specifies the size, in characters, 
        /// of the buffer that is used to store a history of user input into the console window.
        /// </summary>
        public uint HistoryBufferSize { get; set; }

        /// <summary>
        /// NumberOfHistoryBuffers (4 bytes): A 32-bit, unsigned integer that specifies the number of 
        /// history buffers to use.
        /// </summary>
        public uint NumberOfHistoryBuffers { get; set; }

        /// <summary>
        /// HistoryNoDup (4 bytes): A 32-bit, unsigned integer that specifies whether to remove 
        /// duplicates in the history buffer.
        /// 
        /// 0x00000000          Duplicates are not allowed.
        /// 0x00000000 &lt; value  Duplicates are allowed.
        /// </summary>
        public bool HistoryNoDup { get; set; }

        /// <summary>
        /// ColorTable (64 bytes): A table of 16 32-bit, unsigned integers specifying the RGB colors 
        /// that are used for text in the console window. The values of the fill attribute fields 
        /// FillAttributes and PopupFillAttributes are used as indexes into this table to specify the 
        /// final foreground and background color for a character.
        /// </summary>
        public byte[] ColorTable { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] ConsoleDataBlock = new byte[BlockSize];
            Buffer.BlockCopy(BitConverter.GetBytes(BlockSize), 0, ConsoleDataBlock, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)BlockSignature), 0, ConsoleDataBlock, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)FillAttributes), 0, ConsoleDataBlock, 8, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)PopupFillAttributes), 0, ConsoleDataBlock, 10, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(ScreenBufferSizeX), 0, ConsoleDataBlock, 12, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(ScreenBufferSizeY), 0, ConsoleDataBlock, 14, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(WindowSizeX), 0, ConsoleDataBlock, 16, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(WindowSizeY), 0, ConsoleDataBlock, 18, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(WindowOriginX), 0, ConsoleDataBlock, 20, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(WindowOriginY), 0, ConsoleDataBlock, 22, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Unused1), 0, ConsoleDataBlock, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Unused2), 0, ConsoleDataBlock, 28, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(FontSize), 0, ConsoleDataBlock, 32, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)FontFamily | (uint)FontPitch), 0, ConsoleDataBlock, 36, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(FontWeight), 0, ConsoleDataBlock, 40, 4);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(FaceName), 0, ConsoleDataBlock, 44, FaceName.Length < 32 ? FaceName.Length * 2 : 62);
            Buffer.BlockCopy(BitConverter.GetBytes(CursorSize), 0, ConsoleDataBlock, 108, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)(FullScreen ? 1 : 0)), 0, ConsoleDataBlock, 112, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)(QuickEdit ? 1 : 0)), 0, ConsoleDataBlock, 116, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)(InsertMode ? 1 : 0)), 0, ConsoleDataBlock, 120, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)(AutoPosition ? 1 : 0)), 0, ConsoleDataBlock, 124, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(HistoryBufferSize), 0, ConsoleDataBlock, 128, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(NumberOfHistoryBuffers), 0, ConsoleDataBlock, 132, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)(HistoryNoDup ? 1 : 0)), 0, ConsoleDataBlock, 136, 4);
            Buffer.BlockCopy(ColorTable, 0, ConsoleDataBlock, 140, ColorTable.Length < 64 ? ColorTable.Length : 64);

            return ConsoleDataBlock;
        }
        #endregion /// GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("FillAttributes: {0}", FillAttributes);
            builder.AppendLine();
            builder.AppendFormat("PopupFillAttributes: {0}", PopupFillAttributes);
            builder.AppendLine();
            builder.AppendFormat("ScreenBufferSizeX: {0}", ScreenBufferSizeX);
            builder.AppendLine();
            builder.AppendFormat("ScreenBufferSizeY: {0}", ScreenBufferSizeY);
            builder.AppendLine();
            builder.AppendFormat("WindowSizeX: {0}", WindowSizeX);
            builder.AppendLine();
            builder.AppendFormat("WindowSizeY: {0}", WindowSizeY);
            builder.AppendLine();
            builder.AppendFormat("WindowOriginX: {0}", WindowOriginX);
            builder.AppendLine();
            builder.AppendFormat("WindowOriginY: {0}", WindowOriginY);
            builder.AppendLine();
            builder.AppendFormat("FontSize: {0}", FontSize);
            builder.AppendLine();
            builder.AppendFormat("FontFamily: {0}", FontFamily);
            builder.AppendLine();
            builder.AppendFormat("FontPitch: {0}", FontPitch);
            builder.AppendLine();
            builder.AppendFormat("FontWeight: {0}", FontWeight);
            builder.AppendLine();
            builder.AppendFormat("FaceName: {0}", FaceName);
            builder.AppendLine();
            builder.AppendFormat("CursorSize: {0}", CursorSize);
            builder.AppendLine();
            builder.AppendFormat("FullScreen: {0}", FullScreen);
            builder.AppendLine();
            builder.AppendFormat("QuickEdit: {0}", QuickEdit);
            builder.AppendLine();
            builder.AppendFormat("InsertMode: {0}", InsertMode);
            builder.AppendLine();
            builder.AppendFormat("AutoPosition: {0}", AutoPosition);
            builder.AppendLine();
            builder.AppendFormat("HistoryBufferSize: {0} (0x{0:X})", HistoryBufferSize);
            builder.AppendLine();
            builder.AppendFormat("NumberOfHistoryBuffers: {0}", NumberOfHistoryBuffers);
            builder.AppendLine();
            builder.AppendFormat("HistoryNoDup: {0}", HistoryNoDup);
            builder.AppendLine();
            builder.AppendFormat("ColorTable: {0}", BitConverter.ToString(ColorTable).Replace("-", " "));
            builder.AppendLine();
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a ConsoleDataBlock from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A ConsoleDataBlock object</returns>
        public static ConsoleDataBlock FromByteArray(byte[] ba)
        {
            ConsoleDataBlock ConsoleDataBlock = new ConsoleDataBlock();

            if (ba.Length < 0xCC)
            {
                throw new ArgumentException(string.Format("Size of the ConsoleDataBlock Structure is less than 204 ({0})", ba.Length));
            }

            uint BlockSize = BitConverter.ToUInt32(ba, 0);
            if (BlockSize > ba.Length)
            {
                throw new ArgumentException(string.Format("BlockSize is {0} is incorrect (expected {1})", BlockSize, ConsoleDataBlock.BlockSize));
            }

            BlockSignature BlockSignature = (BlockSignature)BitConverter.ToUInt32(ba, 4);
            if (BlockSignature != ConsoleDataBlock.BlockSignature)
            {
                throw new ArgumentException(string.Format("BlockSignature is {0} is incorrect (expected {1})", BlockSignature, ConsoleDataBlock.BlockSignature));
            }

            ConsoleDataBlock.FillAttributes = (FillAttributes)BitConverter.ToUInt16(ba, 8);
            ConsoleDataBlock.PopupFillAttributes = (FillAttributes)BitConverter.ToUInt16(ba, 10);
            ConsoleDataBlock.ScreenBufferSizeX = BitConverter.ToUInt16(ba, 12);
            ConsoleDataBlock.ScreenBufferSizeY = BitConverter.ToUInt16(ba, 14);
            ConsoleDataBlock.WindowSizeX = BitConverter.ToUInt16(ba, 16);
            ConsoleDataBlock.WindowSizeY = BitConverter.ToUInt16(ba, 18);
            ConsoleDataBlock.WindowOriginX = BitConverter.ToUInt16(ba, 20);
            ConsoleDataBlock.WindowOriginY = BitConverter.ToUInt16(ba, 22);
            ConsoleDataBlock.Unused1 = BitConverter.ToUInt32(ba, 24);
            ConsoleDataBlock.Unused2 = BitConverter.ToUInt32(ba, 28);
            ConsoleDataBlock.FontSize = BitConverter.ToUInt32(ba, 32);
            ConsoleDataBlock.FontFamily = (FontFamily)(BitConverter.ToUInt32(ba, 36) & 0xF0);
            ConsoleDataBlock.FontPitch = (FontPitch)(BitConverter.ToUInt32(ba, 36) & 0xF);
            ConsoleDataBlock.FontWeight = BitConverter.ToUInt32(ba, 40);
            byte[] FaceName = new byte[64];
            Buffer.BlockCopy(ba, 44, FaceName, 0, 64);
            ConsoleDataBlock.FaceName = Encoding.Unicode.GetString(FaceName).TrimEnd(new char[] { (char)0 });
            ConsoleDataBlock.CursorSize = BitConverter.ToUInt32(ba, 108);
            ConsoleDataBlock.FullScreen = BitConverter.ToUInt32(ba, 112) != 0;
            ConsoleDataBlock.QuickEdit = BitConverter.ToUInt32(ba, 116) != 0;
            ConsoleDataBlock.InsertMode = BitConverter.ToUInt32(ba, 120) != 0;
            ConsoleDataBlock.AutoPosition = BitConverter.ToUInt32(ba, 124) != 0;
            ConsoleDataBlock.HistoryBufferSize = BitConverter.ToUInt32(ba, 128);
            ConsoleDataBlock.NumberOfHistoryBuffers = BitConverter.ToUInt32(ba, 132);
            ConsoleDataBlock.HistoryNoDup = BitConverter.ToUInt32(ba, 136) != 0;
            Buffer.BlockCopy(ba, 140, ConsoleDataBlock.ColorTable, 0, 64);

            return ConsoleDataBlock;
        }
        #endregion // FromByteArray
    }
}
