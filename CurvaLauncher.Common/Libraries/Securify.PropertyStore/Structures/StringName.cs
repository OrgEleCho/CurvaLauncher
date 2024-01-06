using CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Flags;
using System;
using System.Linq;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Structures
{
    /// <summary>
    /// The Serialized Property Value (String Name) structure specifies a single 
    /// property within a Serialized Property Storage structure, where the property 
    /// is identified by a unique Unicode string.
    /// </summary>
    public class StringName : SerializedPropertyValue
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public StringName() : base() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name">A null-terminated Unicode string that specifies the identity of the property</param>
        /// <param name="TypedPropertyValue">A TypedPropertyValue structure</param>
        public StringName(string Name, TypedPropertyValue TypedPropertyValue) : base()
        {
            this.Name = Name;
            this.TypedPropertyValue = TypedPropertyValue;
        }
        #endregion // StringName

        /// <summary>
        /// Value Size (4 bytes): An unsigned integer that specifies the total size, in bytes, 
        /// of this structure. It MUST be 0x00000000 if this is the last The Serialized Property 
        /// Value in the enclosing Serialized Property Storage structure.
        /// </summary>
        /// <returns></returns>
        public override uint ValueSize => 9 + NameSize + (uint)TypedPropertyValue.GetBytes().Length;

        /// <summary>
        /// Name Size (4 bytes): An unsigned integer that specifies the size, in bytes, 
        /// of the Name field, including the null-terminating character.
        /// </summary>
        public uint NameSize => ((uint)Name.Length + 1) * 2;

        /// <summary>
        /// Reserved(1 byte): MUST be 0x00.
        /// </summary>
        public byte Reserved => 0x00;

        /// <summary>
        /// Name (variable): A null-terminated Unicode string that specifies the identity 
        /// of the property. It MUST be unique within the enclosing Serialized Property 
        /// Storage structure.
        /// </summary>
        public string Name { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] PropertyValue = new byte[ValueSize];
            Buffer.BlockCopy(BitConverter.GetBytes(ValueSize), 0, PropertyValue, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(NameSize), 0, PropertyValue, 4, 4);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(Name), 0, PropertyValue, 9, 4);
            Buffer.BlockCopy(TypedPropertyValue.GetBytes(), 0, PropertyValue, 9 + (int)NameSize, TypedPropertyValue.GetBytes().Length);
            return PropertyValue;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("ValueSize: {0} (0x{0:X})", ValueSize);
            builder.AppendLine();
            builder.AppendFormat("NameSize: {0} (0x{0:X})", NameSize);
            builder.AppendLine();
            builder.AppendFormat("Name: {0}", Name);
            builder.AppendLine();
            builder.Append(TypedPropertyValue.ToString());
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create an SerializedPropertyValue from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>An SerializedPropertyValue object</returns>
        public static SerializedPropertyValue FromByteArray(byte[] ba)
        {
            StringName StringName = new StringName();

            if (ba.Length < 9)
            {
                throw new ArgumentException(string.Format("Size of the StringName is less than 9 ({0})", ba.Length));
            }

            uint ValueSize = BitConverter.ToUInt32(ba, 0);
            if (ba.Length < ValueSize)
            {
                throw new ArgumentException(string.Format("Size of the StringName is not equal to {0} ({1})", ValueSize, ba.Length));
            }

            uint NameSize = BitConverter.ToUInt32(ba, 4);
            if (ba.Length - 9 < NameSize)
            {
                throw new ArgumentException(string.Format("Size of the NameSize is not equal to {0} ({1})", ValueSize, ba.Length - 8));
            }

            byte[] Name = ba.Skip(9).Take((int)NameSize).ToArray();
            StringName.Name = Encoding.Unicode.GetString(Name).TrimEnd(new char[] { (char)0 });

            PropertyType Type = (PropertyType)BitConverter.ToUInt16(ba, 9);
            byte[] Value = ba.Skip(9 + (int)NameSize).Take((int)(ValueSize - 9 - (int)NameSize)).ToArray();
            StringName.TypedPropertyValue = new TypedPropertyValue(Type, Value);

            return StringName;
        }
        #endregion // FromByteArray
    }
}
