using System;
using System.Collections;
using System.Text;
using CurvaLauncher.Libraries.Securify.ShellLink.Flags;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The ShellLinkHeader structure contains identification information, timestamps, 
    /// and flags that specify the presence of optional structures, including LinkTargetIDList, 
    /// LinkInfo, and StringData.
    /// </summary>
    public class ShellLinkHeader : Structure
    {
        /// <summary>
        /// HeaderSize (4 bytes): The size, in bytes, of this structure. This value MUST be 0x0000004C
        /// </summary>
        public readonly uint HeaderSize = 0x4C;

        /// <summary>
        /// LinkCLSID (16 bytes): A class identifier (CLSID). This value MUST be 
        /// 00021401-0000-0000-C000-000000000046.
        /// </summary>
        public readonly Guid LinkCLSID = new Guid("00021401-0000-0000-C000-000000000046");

        /// <summary>
        /// A value that MUST be zero.
        /// </summary>
        public readonly ushort Reserved1 = 0;

        /// <summary>
        /// A value that MUST be zero.
        /// </summary>
        public readonly uint Reserved2 = 0;

        /// <summary>
        /// A value that MUST be zero.
        /// </summary>
        public readonly uint Reserved3 = 0;

        #region Constructor
        /// <summary>
        /// ShellLinkHeader Constructor
        /// </summary>
        public ShellLinkHeader()
        {
            //CreationTime = DateTime.Now.ToFileTime();
            //AccessTime = DateTime.Now.ToFileTime();
            //sWriteTime = DateTime.Now.ToFileTime();
            IconIndex = -1;
            ShowCommand = ShowCommand.SW_SHOWNORMAL;
            HotKey = new HotKeyFlags();
        }
        #endregion // Constructor

        /// <summary>
        /// LinkFlags (4 bytes): A LinkFlags structure that specifies information about the shell
        /// link and the presence of optional portions of the structure.
        /// </summary>
        public virtual LinkFlags LinkFlags { get; set; }

        /// <summary>
        /// FileAttributes (4 bytes): A FileAttributesFlags structure that specifies information
        /// about the link target.
        /// </summary>
        public FileAttributesFlags FileAttributes { get; set; }

        /// <summary>
        /// CreationTime (8 bytes): A FILETIME structure that specifies the creation time of the link 
        /// target in UTC (Coordinated Universal Time). If the value is zero, there is no creation time 
        /// set on the link target.
        /// </summary>
        public long CreationTime { get; set; }

        /// <summary>
        /// AccessTime (8 bytes): A FILETIME structure that specifies the access time of the link target 
        /// in UTC (Coordinated Universal Time). If the value is zero, there is no access time set on the 
        /// link target.
        /// </summary>
        public long AccessTime { get; set; }

        /// <summary>
        /// WriteTime (8 bytes): A FILETIME structure that specifies the write time of the link target in 
        /// UTC (Coordinated Universal Time). If the value is zero, there is no write time set on the link 
        /// target.
        /// </summary>
        public long WriteTime { get; set; }

        /// <summary>
        /// FileSize (4 bytes): A 32-bit unsigned integer that specifies the size, in bytes, of the link 
        /// target. If the link target file is larger than 0xFFFFFFFF, this value specifies the least 
        /// significant 32 bits of the link target file size.
        /// </summary>
        public uint FileSize { get; set; }

        /// <summary>
        /// IconIndex (4 bytes): A 32-bit signed integer that specifies the index of an icon within a given 
        /// icon location.
        /// </summary>
        public int IconIndex { get; set; }

        /// <summary>
        /// ShowCommand (4 bytes): A 32-bit unsigned integer that specifies the expected window state of an 
        /// application launched by the link.
        /// </summary>
        public ShowCommand ShowCommand { get; set; }

        /// <summary>
        /// HotKey (2 bytes): A HotKeyFlags structure that specifies the keystrokes used to launch the application 
        /// referenced by the shortcut key. This value is assigned to the application after it is launched, so that 
        /// pressing the key activates that application.
        /// </summary>
        public HotKeyFlags HotKey { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] Header = new byte[HeaderSize];
            Buffer.BlockCopy(BitConverter.GetBytes(HeaderSize), 0, Header, 0, 4);
            Buffer.BlockCopy(LinkCLSID.ToByteArray(), 0, Header, 4, 16);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)LinkFlags), 0, Header, 20, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)FileAttributes), 0, Header, 24, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(CreationTime), 0, Header, 28, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(AccessTime), 0, Header, 36, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(WriteTime), 0, Header, 44, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(FileSize), 0, Header, 52, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(IconIndex), 0, Header, 56, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)ShowCommand), 0, Header, 60, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(HotKey.HotKey), 0, Header, 64, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Reserved1), 0, Header, 66, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Reserved2), 0, Header, 68, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(Reserved3), 0, Header, 72, 4);
            return Header;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("HeaderSize: {0} (0x{0:X})", HeaderSize);
            builder.AppendLine();
            builder.AppendFormat("LinkCLSID: {0}", LinkCLSID);
            builder.AppendLine();
            builder.AppendFormat("LinkFlags: {0}", LinkFlags);
            builder.AppendLine();
            builder.AppendFormat("FileAttributes: {0}", FileAttributes);
            builder.AppendLine();
            builder.AppendFormat("CreationTime: {0}", DateTime.FromFileTimeUtc(CreationTime));
            builder.AppendLine();
            builder.AppendFormat("AccessTime: {0}", DateTime.FromFileTimeUtc(AccessTime));
            builder.AppendLine();
            builder.AppendFormat("WriteTime: {0}", DateTime.FromFileTimeUtc(WriteTime));
            builder.AppendLine();
            builder.AppendFormat("FileSize: {0} (0x{0:X})", FileSize);
            builder.AppendLine();
            builder.AppendFormat("IconIndex: {0}", IconIndex);
            builder.AppendLine();
            builder.AppendFormat("ShowCommand: {0}", ShowCommand);
            builder.AppendLine();
            if (HotKey.HighByte != 0)
            {
                builder.AppendFormat("HotKey: {0} + {1}", HotKey.HighByte, HotKey.LowByte);
                builder.AppendLine();
            }
            else
            {
                builder.AppendFormat("HotKey: {0}", HotKey.LowByte);
                builder.AppendLine();
            }

            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a ShellLinkHeader from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A ShellLinkHeader object</returns>
        public static ShellLinkHeader FromByteArray(byte[] ba)
        {
            ShellLinkHeader Header = new ShellLinkHeader();

            if (ba.Length < Header.HeaderSize)
            {
                throw new ArgumentException(string.Format("Size of the LNK Header is less than {0} ({1})", Header.HeaderSize, ba.Length));
            }

            if (BitConverter.ToUInt32(ba, 0) != Header.HeaderSize)
            {
                throw new ArgumentException(string.Format("The LNK Header Size is {0} is incorrect (expected {1})", BitConverter.ToUInt32(ba, 0), Header.HeaderSize));
            }

            byte[] linkCLSID = new byte[16];
            Buffer.BlockCopy(ba, 4, linkCLSID, 0, linkCLSID.Length);
            if (!StructuralComparisons.StructuralEqualityComparer.Equals(Header.LinkCLSID.ToByteArray(), linkCLSID))
            {
                throw new ArgumentException(string.Format("The LNK CLSID is {0} is incorrect (expected {1})", new Guid(linkCLSID), Header.LinkCLSID));
            }

            Header.LinkFlags = (LinkFlags)BitConverter.ToUInt32(ba, 20);
            Header.FileAttributes = (FileAttributesFlags)BitConverter.ToUInt32(ba, 24);
            Header.CreationTime = BitConverter.ToInt64(ba, 28);
            Header.AccessTime = BitConverter.ToInt64(ba, 36);
            Header.WriteTime = BitConverter.ToInt64(ba, 44);
            Header.FileSize = BitConverter.ToUInt32(ba, 52);
            Header.IconIndex = BitConverter.ToInt32(ba, 56);
            Header.ShowCommand = (ShowCommand)BitConverter.ToUInt32(ba, 60);
            Header.HotKey = new HotKeyFlags(BitConverter.ToUInt16(ba, 64));
            return Header;
        }
        #endregion // FromByteArray
    }
}
