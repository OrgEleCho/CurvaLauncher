using CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Flags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Structures
{
    /// <summary>
    /// The Serialized Property Value (Integer Name) structure specifies a single property 
    /// within a Serialized Property Storage structure, where the property is identified 
    /// by a unique unsigned integer.
    /// </summary>
    public class IntegerName : SerializedPropertyValue
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public IntegerName() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ID">An unsigned integer that specifies the identity of the property.</param>
        /// <param name="TypedPropertyValue">A TypedPropertyValue structure</param>
        public IntegerName(uint ID, TypedPropertyValue TypedPropertyValue) : base()
        {
            this.ID = ID;
            this.TypedPropertyValue = TypedPropertyValue;
        }
        #endregion // IntegerName

        /// <summary>
        /// Value Size (4 bytes): An unsigned integer that specifies the total size, in bytes, 
        /// of this structure. It MUST be 0x00000000 if this is the last The Serialized Property 
        /// Value in the enclosing Serialized Property Storage structure.
        /// </summary>
        /// <returns></returns>
        public override uint ValueSize => 9 + (uint)TypedPropertyValue.GetBytes().Length;

        /// <summary>
        /// Id (4 bytes): An unsigned integer that specifies the identity of the property. 
        /// It MUST be unique within the enclosing Serialized Property Storage structure.
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// Reserved(1 byte): MUST be 0x00.
        /// </summary>
        public byte Reserved => 0x00;

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] PropertyValue = new byte[ValueSize];
            Buffer.BlockCopy(BitConverter.GetBytes(ValueSize), 0, PropertyValue, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(ID), 0, PropertyValue, 4, 4);
            Buffer.BlockCopy(TypedPropertyValue.GetBytes(), 0, PropertyValue, 9, TypedPropertyValue.GetBytes().Length);
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
            builder.AppendFormat("ID: 0x{0:X}", ID);
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
            IntegerName IntegerName = new IntegerName();

            if (ba.Length < 9)
            {
                throw new ArgumentException(string.Format("Size of the StringName is less than 9 ({0})", ba.Length));
            }

            uint ValueSize = BitConverter.ToUInt32(ba, 0);
            if (ba.Length < ValueSize)
            {
                throw new ArgumentException(string.Format("Size of the StringName is not equal to {0} ({1})", ValueSize, ba.Length));
            }

            IntegerName.ID = BitConverter.ToUInt32(ba, 4);
            PropertyType Type = (PropertyType)BitConverter.ToUInt16(ba, 9);
            byte[] Value = ba.Skip(13).Take((int)(ValueSize - 13)).ToArray();
            IntegerName.TypedPropertyValue = new TypedPropertyValue(Type, Value);

            return IntegerName;
        }
        #endregion // FromByteArray
    }
}
