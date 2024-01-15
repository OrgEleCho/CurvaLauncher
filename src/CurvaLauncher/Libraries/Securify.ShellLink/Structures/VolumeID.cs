using System;
using System.Text;
using System.Linq;
using CurvaLauncher.Libraries.Securify.ShellLink.Flags;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The VolumeID structure specifies information about the volume that a link target was on when the link was 
    /// created. This information is useful for resolving the link if the file is not found in its original location.
    /// </summary>
    public class VolumeID : Structure
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public VolumeID() : this(false) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IsUnicode">When set to true, wide chars will be used</param>
        public VolumeID(bool IsUnicode) : base()
        {
            this.IsUnicode = IsUnicode;
            VolumeLabel = "";
        }
        #endregion // Constructor

        /// <summary>
        /// When set to true, wide chars will be used
        /// </summary>
        public bool IsUnicode { get; set; }

        /// <summary>
        /// VolumeIDSize (4 bytes): A 32-bit, unsigned integer that specifies the size, in bytes, of this structure. 
        /// This value MUST be greater than 0x00000010. All offsets specified in this structure MUST be less than this 
        /// value, and all strings contained in this structure MUST fit within the extent defined by this size.
        /// </summary>
        public uint VolumeIDSize => (uint)(IsUnicode ? 0x14 + (VolumeLabel.Length + 1) * 2 : 0x10 + VolumeLabel.Length + 1);

        /// <summary>
        /// DriveType (4 bytes): A 32-bit, unsigned integer that specifies the type of drive the link target is stored on.
        /// </summary>
        public DriveType DriveType { get; set; }

        /// <summary>
        /// DriveSerialNumber (4 bytes): A 32-bit, unsigned integer that specifies the drive serial number of the volume the 
        /// link target is stored on.
        /// </summary>
        public uint DriveSerialNumber { get; set; }

        /// <summary>
        /// VolumeLabelOffset (4 bytes): A 32-bit, unsigned integer that specifies the location of a string that contains 
        /// the volume label of the drive that the link target is stored on. This value is an offset, in bytes, from the 
        /// start of the VolumeID structure to a NULL-terminated string of characters, defined by the system default code page. 
        /// The volume label string is located in the Data field of this structure.
        /// 
        /// If the value of this field is 0x00000014, it MUST be ignored, and the value of the VolumeLabelOffsetUnicode field 
        /// MUST be used to locate the volume label string.
        /// </summary>
        public uint VolumeLabelOffset => (uint)(IsUnicode ? 0x14 : 0x10);

        /// <summary>
        /// VolumeLabelOffsetUnicode (4 bytes): An optional, 32-bit, unsigned integer that specifies the location of a string 
        /// that contains the volume label of the drive that the link target is stored on. This value is an offset, in bytes, 
        /// from the start of the VolumeID structure to a NULL-terminated string of Unicode characters. The volume label string 
        /// is located in the Data field of this structure.
        /// 
        /// If the value of the VolumeLabelOffset field is not 0x00000014, this field MUST be ignored, and the value of the 
        /// VolumeLabelOffset field MUST be used to locate the volume label string.
        /// </summary>
        public uint VolumeLabelOffsetUnicode => (uint)(IsUnicode ? 0x14 : 0);

        /// <summary>
        /// The volume label of the drive as a string
        /// </summary>
        public string VolumeLabel { get; set; }

        /// <summary>
        /// Data (variable): A buffer of data that contains the volume label of the drive as a string defined by the system 
        /// default code page or Unicode characters, as specified by preceding fields.
        /// </summary>
        public byte[] Data => IsUnicode ? Encoding.Unicode.GetBytes(VolumeLabel) : Encoding.Default.GetBytes(VolumeLabel);

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] VolumeId = new byte[VolumeIDSize];
            Buffer.BlockCopy(BitConverter.GetBytes(VolumeIDSize), 0, VolumeId, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)DriveType), 0, VolumeId, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(DriveSerialNumber), 0, VolumeId, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(VolumeLabelOffset), 0, VolumeId, 12, 4);
            if (VolumeLabelOffset == 0x14)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(VolumeLabelOffsetUnicode), 0, VolumeId, 16, 4);
                Buffer.BlockCopy(Encoding.Unicode.GetBytes(VolumeLabel), 0, VolumeId, 0x14, VolumeLabel.Length * 2);
            }
            else
            {
                Buffer.BlockCopy(Encoding.Default.GetBytes(VolumeLabel), 0, VolumeId, 0x10, VolumeLabel.Length);
            }
            return VolumeId;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("VolumeIDSize: {0} (0x{0:X})", VolumeIDSize);
            builder.AppendLine();
            builder.AppendFormat("DriveType: {0}", DriveType);
            builder.AppendLine();
            builder.AppendFormat("DriveSerialNumber: 0x{0:X}", DriveSerialNumber);
            builder.AppendLine();
            builder.AppendFormat("VolumeLabelOffset: {0} (0x{0:X})", VolumeLabelOffset);
            builder.AppendLine();
            if (VolumeLabelOffset == 0x14)
            {
                builder.AppendFormat("VolumeLabelOffsetUnicode: {0} (0x{0:X})", VolumeLabelOffsetUnicode);
                builder.AppendLine();
            }
            builder.AppendFormat("VolumeLabel: {0}", VolumeLabel);
            builder.AppendLine();
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a VolumeID from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A VolumeID object</returns>
        public static VolumeID FromByteArray(byte[] ba)
        {
            VolumeID VolumeId = new VolumeID(false);
            if (ba.Length <= 0x10)
            {
                throw new ArgumentException(string.Format("Size of the VolumeID Structure is less than 17 ({0})", ba.Length));
            }

            uint VolumeIDSize = BitConverter.ToUInt32(ba, 0);
            if (VolumeIDSize > ba.Length)
            {
                throw new ArgumentException(string.Format("The VolumeIDSize is {0} is incorrect (expected {1})", VolumeIDSize, ba.Length));
            }

            VolumeId.DriveType = (DriveType)BitConverter.ToUInt32(ba, 4);
            VolumeId.DriveSerialNumber = BitConverter.ToUInt32(ba, 8);
            uint VolumeLabelOffset = BitConverter.ToUInt32(ba, 12);

            if (VolumeLabelOffset == 0x14)
            {
                uint VolumeLabelOffsetUnicode = BitConverter.ToUInt32(ba, 16);
                byte[] Data = new byte[VolumeIDSize - 0x14];
                Buffer.BlockCopy(ba, 0x14, Data, 0, Data.Length);
                VolumeId.VolumeLabel = Encoding.Unicode.GetString(Data).TrimEnd(new char[] { (char)0 });
                VolumeId.IsUnicode = true;
            }
            else
            {
                byte[] Data = new byte[VolumeIDSize - 0x10];
                Buffer.BlockCopy(ba, 0x10, Data, 0, Data.Length);
                VolumeId.VolumeLabel = Encoding.Default.GetString(Data).TrimEnd(new char[] { (char)0 });
            }
            return VolumeId;
        }
        #endregion // FromByteArray
    }
}
