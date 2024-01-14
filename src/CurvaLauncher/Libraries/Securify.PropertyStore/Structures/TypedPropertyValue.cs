using System;
using System.Text;
using System.Linq;
using CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Flags;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Structures
{
    /// <summary>
    /// The TypedPropertyValue structure represents the typed value of a property in a property set.
    /// </summary>
    public class TypedPropertyValue : Structure
    {
        private PropertyType _Type;
        private byte[] _Value;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public TypedPropertyValue(PropertyType type, byte[] value) : base()
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));

            _Type = type;
            _Value = value;
            // TODO validate Type against Value
        }
        #endregion // Constructor

        /// <summary>
        /// Type (2 bytes): MUST be a value from the PropertyType enumeration, indicating the type 
        /// of property represented.
        /// </summary>
        public PropertyType Type => _Type;

        /// <summary>
        /// Padding (2 bytes): MUST be set to zero, and any nonzero value SHOULD be rejected.
        /// </summary>
        public ushort Padding => 0x0000;

        #region Value
        /// <summary>
        /// Value (variable): MUST be the value of the property represented and serialized 
        /// according to the value of Type 
        /// </summary>
        public object Value
        {
            get
            {
                // TODO Array / Vector
                switch (Type)
                {
                    case PropertyType.VT_I1:
                        return (sbyte)_Value[0];
                    case PropertyType.VT_UI1:
                        return _Value[0];
                    case PropertyType.VT_I2:
                        return BitConverter.ToInt16(_Value, 0);
                    case PropertyType.VT_UI2:
                        return BitConverter.ToUInt16(_Value, 0);
                    case PropertyType.VT_INT:
                    case PropertyType.VT_I4:
                        return BitConverter.ToInt32(_Value, 0);
                    case PropertyType.VT_ERROR:
                    case PropertyType.VT_UINT:
                    case PropertyType.VT_UI4:
                        return BitConverter.ToUInt32(_Value, 0);
                    case PropertyType.VT_I8:
                        return BitConverter.ToInt64(_Value, 0);
                    case PropertyType.VT_CY:
                    case PropertyType.VT_UI8:
                        return BitConverter.ToUInt64(_Value, 0);
                    case PropertyType.VT_R4:
                        return BitConverter.ToSingle(_Value, 0);
                    case PropertyType.VT_R8:
                        return BitConverter.ToDouble(_Value, 0);
                    case PropertyType.VT_DATE:
                    case PropertyType.VT_FILETIME:
                        return DateTime.FromFileTimeUtc(BitConverter.ToInt64(_Value, 0));
                    case PropertyType.VT_BOOL:
                        return _Value[0] != 0x00 || _Value[1] != 0x00;
                    case PropertyType.VT_LPSTR:
                        return Encoding.Default.GetString(_Value.Skip(4).ToArray()).TrimEnd(new char[] { (char)0 });
                    case PropertyType.VT_LPWSTR:
                        return Encoding.Unicode.GetString(_Value.Skip(4).ToArray()).TrimEnd(new char[] { (char)0 });
                    case PropertyType.VT_CLSID:
                        return new Guid(_Value);
                    case PropertyType.VT_STREAM: // Not supported
                    case PropertyType.VT_STORAGE: // Not supported
                    case PropertyType.VT_STREAMED_Object: // Not supported
                    case PropertyType.VT_STORED_Object: // Not supported
                    case PropertyType.VT_BLOB_Object: // Not supported
                    case PropertyType.VT_VERSIONED_STREAM: // Not supported
                    case PropertyType.VT_BLOB: // TODO
                    case PropertyType.VT_DECIMAL: // TODO
                    case PropertyType.VT_BSTR: // TODO
                    case PropertyType.VT_CF: // TODO
                    default:
                        return _Value;
                }
            }
        }
        #endregion // Value

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] Value = new byte[_Value.Length + 4];
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)Type), 0, Value, 0, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(Padding), 0, Value, 2, 2);
            if (_Value != null)
            {
                Buffer.BlockCopy(_Value, 0, Value, 4, _Value.Length);
            }
            return Value;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("Type: {0}", Type);
            builder.AppendLine();
            builder.AppendFormat("Value: {0}", Value.ToString());
            builder.AppendLine();
            return builder.ToString();
        }
        #endregion // ToString
    }
}
