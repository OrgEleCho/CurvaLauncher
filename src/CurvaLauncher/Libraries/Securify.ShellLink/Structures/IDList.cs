using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using CurvaLauncher.Libraries.Securify.ShellLink.Internal;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// The stored IDList structure specifies the format of a persisted item ID list.
    /// </summary>
    public class IDList : Structure
    {
        /// <summary>
        /// TerminalID (2 bytes): A 16-bit, unsigned integer that indicates the end of the item IDs.
        /// This value MUST be zero.
        /// </summary>
        public readonly byte[] TerminalID = new byte[] { 0x00, 0x00 };

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public IDList() : this(new List<ItemID>()) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="idList">An IDList</param>
        public IDList(List<ItemID> idList) { ItemIDList = idList; }
        #endregion // Constructor

        /// <summary>
        /// ItemIDList (variable): An array of zero or more ItemID structures.
        /// </summary>
        public List<ItemID> ItemIDList { get; set; }

        #region IDListSize
        /// <summary>
        /// IDListSize (2 bytes): The size, in bytes, of the IDList field.
        /// </summary>
        public ushort IDListSize
        {
            get
            {
                ushort Size = 2;
                for (int i = 0; i < ItemIDList.Count; i++)
                {
                    Size += ItemIDList[i].ItemIDSize;
                }
                return Size;
            }
        }
        #endregion // IDListSize

        #region Bytes
        private byte[] Bytes
        {
            get
            {
                byte[] idList = new byte[IDListSize];
                int Offset = 0;
                for (int i = 0; i < ItemIDList.Count; i++)
                {
                    ItemID item = ItemIDList[i];
                    Buffer.BlockCopy(item.GetBytes(), 0, idList, Offset, item.ItemIDSize);
                    Offset += item.ItemIDSize;
                }
                Buffer.BlockCopy(TerminalID, 0, idList, Offset, 2);
                return idList;
            }
        }
        #endregion // Bytes

        #region Path
        /// <summary>
        /// Returns the path defined by the item identifier list
        /// </summary>
        public string Path
        {
            get
            {
                byte[] pszPath = new byte[Win32.MAX_PATH * 2];
                if (!Win32.SHGetPathFromIDListW(Bytes, pszPath))
                {
                    // error
                }
                return Encoding.Unicode.GetString(pszPath).TrimEnd(new char[] { (char)0 });
            }

            set
            {
                ItemIDList.Clear();
                IntPtr pIdList = Win32.ILCreateFromPath(value);

                if (pIdList == IntPtr.Zero)
                {
                    // FIXME SHSimpleIDListFromPath is deprecated
                    pIdList = Win32.SHSimpleIDListFromPath(value);
                }

                if (pIdList != IntPtr.Zero)
                {
                    ushort cb;
                    IntPtr ptr = pIdList;
                    while ((cb = (ushort)Marshal.ReadInt16(ptr)) > 2)
                    {
                        ptr += 2;
                        cb -= 2;
                        byte[] abID = new byte[cb];
                        Marshal.Copy(ptr, abID, 0, cb);
                        ItemID itemId = new ItemID(abID);
                        ItemIDList.Add(itemId);
                        ptr += cb;
                    }

                    Win32.ILFree(pIdList);
                }
            }
        }
        #endregion // Path

        #region DisplayName
        /// <summary>
        /// Retrieves the display name of an item identified by its IDList.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (Win32.SHGetNameFromIDList(Bytes, SIGDN.SIGDN_NORMALDISPLAY, out IntPtr pszName) == 0)
                {
                    try
                    {
                        return Marshal.PtrToStringAuto(pszName)!;
                    }
                    catch (Exception)
                    {
                        return "";
                    }
                    finally
                    {
                        Win32.CoTaskMemFree(pszName);
                    }
                }
                return "";
            }
        }
        #endregion // DisplayName

        #region GetOffsetByIndex
        /// <summary>
        /// Gets the offset in bytes into the ItemIDList
        /// </summary>
        /// <param name="Index">Index into the ItemIDList</param>
        /// <returns></returns>
        public uint GetOffsetByIndex(int Index)
        {
            uint Offset = 0;

            if (Index < 0 || Index >= ItemIDList.Count)
            {
                throw new ArgumentOutOfRangeException(string.Format("Invalid index {0} provided", Index));
            }

            for (int i = 0; i < Index; i++)
            {
                Offset += ItemIDList[i].ItemIDSize;
            }

            return Offset;
        }
        #endregion // GetOffsetByIndex

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            return Bytes;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            builder.AppendFormat("IDListSize: {0} (0x{0:X})", IDListSize);
            builder.AppendLine();
            builder.AppendFormat("DisplayName: {0}", DisplayName);
            builder.AppendLine();
            builder.AppendFormat("Path: {0}", Path);
            builder.AppendLine();
            return builder.ToString();
        }
        #endregion // ToString
    }
}
