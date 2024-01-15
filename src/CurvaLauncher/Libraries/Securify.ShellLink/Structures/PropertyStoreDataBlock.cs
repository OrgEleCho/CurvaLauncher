using System;
using System.Text;
using System.Collections.Generic;
using CurvaLauncher.Libraries.Securify.ShellLink.PropertyStore;
using CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Structures;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// A PropertyStoreDataBlock structure specifies a set of properties that can be used by 
    /// applications to store extra data in the shell link.
    /// </summary>
    public class PropertyStoreDataBlock : ExtraDataBlock
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyStoreDataBlock() : base()
        {
            PropertyStore = new List<SerializedPropertyStorage>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="PropertyStore">A serialized property storage structure</param>
        public PropertyStoreDataBlock(List<SerializedPropertyStorage> PropertyStore) : base()
        {
            this.PropertyStore = PropertyStore;
        }
        #endregion // Constructor

        #region BlockSize
        /// <summary>
        /// BlockSize (4 bytes): A 32-bit, unsigned integer that specifies the size of the 
        /// PropertyStoreDataBlock structure. This value MUST be greater than or equal to 0x0000000C.
        /// </summary>
        public override uint BlockSize
        {
            get
            {
                uint Size = 12;
                for (int i = 0; i < PropertyStore.Count; i++)
                {
                    Size += PropertyStore[i].StorageSize;
                }
                return Size;
            }
        }
        #endregion // BlockSize

        /// <summary>
        /// BlockSignature (4 bytes): A 32-bit, unsigned integer that specifies the signature of the 
        /// PropertyStoreDataBlock extra data section. This value MUST be 0xA0000009.
        /// </summary>
        public override BlockSignature BlockSignature => BlockSignature.PROPERTY_STORE_PROPS;

        /// <summary>
        /// PropertyStore (variable): A serialized property storage structure
        /// </summary>
        public List<SerializedPropertyStorage> PropertyStore { get; set; }

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            int Offset = 8;
            byte[] PropertyStoreDataBlock = new byte[BlockSize];
            Buffer.BlockCopy(BitConverter.GetBytes(BlockSize), 0, PropertyStoreDataBlock, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)BlockSignature), 0, PropertyStoreDataBlock, 4, 4);
            for (int i = 0; i < PropertyStore.Count; i++)
            {
                SerializedPropertyStorage PropertyStorage = PropertyStore[i];
                Buffer.BlockCopy(PropertyStorage.GetBytes(), 0, PropertyStoreDataBlock, Offset, (int)PropertyStorage.StorageSize);
                Offset += (int)PropertyStorage.StorageSize;
            }
            return PropertyStoreDataBlock;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            for (int i = 0; i < PropertyStore.Count; i++)
            {
                builder.Append(PropertyStore[i].ToString());
            }
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a PropertyStoreDataBlock from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A PropertyStoreDataBlock object</returns>
        public static PropertyStoreDataBlock FromByteArray(byte[] ba)
        {
            PropertyStoreDataBlock PropertyStoreDataBlock = new PropertyStoreDataBlock();
            if (ba.Length < 0xC)
            {
                throw new ArgumentException(string.Format("Size of the PropertyStoreDataBlock Structure is less than 12 ({0})", ba.Length));
            }

            uint BlockSize = BitConverter.ToUInt32(ba, 0);
            if (BlockSize > ba.Length)
            {
                throw new ArgumentException(string.Format("BlockSize is {0} is incorrect (expected {1})", BlockSize, PropertyStoreDataBlock.BlockSize));
            }

            BlockSignature BlockSignature = (BlockSignature)BitConverter.ToUInt32(ba, 4);
            if (BlockSignature != PropertyStoreDataBlock.BlockSignature)
            {
                throw new ArgumentException(string.Format("BlockSignature is {0} is incorrect (expected {1})", BlockSignature, PropertyStoreDataBlock.BlockSignature));
            }

            byte[] data = new byte[BlockSize - 4];
            Buffer.BlockCopy(BitConverter.GetBytes(BlockSize - 4), 0, data, 0, 4);
            Buffer.BlockCopy(ba, 8, data, 4, (int)BlockSize - 8);
            SerializedPropertyStore SerializedPropertyStore = SerializedPropertyStore.FromByteArray(data);
            PropertyStoreDataBlock.PropertyStore = SerializedPropertyStore.PropertyStorage;

            return PropertyStoreDataBlock;
        }
        #endregion // FromByteArray
    }
}
