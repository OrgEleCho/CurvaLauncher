using System;
using System.Collections.Generic;
using System.Linq;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The LinkTargetIDList structure specifies the target of the link. The presence of this 
    /// optional structure is specified by the HasLinkTargetIDList bit in the ShellLinkHeader
    /// </summary>
    public class LinkTargetIDList : IDList
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LinkTargetIDList() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="idList">An IDList</param>
        public LinkTargetIDList(List<ItemID> idList) : base(idList) { }
        #endregion // Constructor

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            byte[] IdList = new byte[IDListSize + 2];
            Buffer.BlockCopy(BitConverter.GetBytes(IDListSize), 0, IdList, 0, 2);
            Buffer.BlockCopy(base.GetBytes(), 0, IdList, 2, IDListSize - 2);
            return IdList;
        }
        #endregion // GetBytes

        #region FromByteArray
        /// <summary>
        /// Create a LinkTargetIDList from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A LinkTargetIDList object</returns>
        public static LinkTargetIDList FromByteArray(byte[] ba)
        {
            LinkTargetIDList IdList = new LinkTargetIDList();

            if (ba.Length < 4)
            {
                throw new ArgumentException(string.Format("Size of the LinkTargetIDList is less than 4 ({0})", ba.Length));
            }

            ushort IDListSize = BitConverter.ToUInt16(ba, 0);
            if (ba.Length < IDListSize)
            {
                throw new ArgumentException(string.Format("Size of the LinkTargetIDList is less than {0} ({1})", IDListSize, ba.Length));
            }

            IDListSize -= 2;
            ba = ba.Skip(2).ToArray();
            while (IDListSize > 2)
            {
                ItemID itemId = ItemID.FromByteArray(ba);
                ushort ItemIDSize = BitConverter.ToUInt16(ba, 0);
                IdList.ItemIDList.Add(itemId);
                IDListSize -= ItemIDSize;
                ba = ba.Skip(ItemIDSize).ToArray();
            }

            return IdList;
        }
        #endregion // FromByteArray
    }
}
