using System;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// Helper class to create an IDList to a Control Panel item
    /// </summary>
    public class CplLinkTargetIDList : LinkTargetIDList
    {
        private const string ThisPc = "{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
        private const string AllControlPanelItems = "{21EC2020-3AEA-1069-A2DD-08002B30309D}";

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the CPL file</param>
        public CplLinkTargetIDList(string Path) : this(Path, "", "") { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the CPL file</param>
        /// <param name="DisplayName">Display Name of the Control Panel item</param>
        public CplLinkTargetIDList(string Path, string DisplayName) : this(Path, DisplayName, "") { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Path">Path to the CPL file</param>
        /// <param name="DisplayName">Display Name of the Control Panel item</param>
        /// <param name="Comment">Control Panel item description</param>
        public CplLinkTargetIDList(string Path, string DisplayName, string Comment) : base()
        {
            this.Path = string.Format(@"::{0}\::{1}", ThisPc, AllControlPanelItems);
            byte[] Data = new byte[22 + (Path.Length + DisplayName.Length + Comment.Length + 3) * 2];
            Data[11] = 0x6a;
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)Path.Length + 1), 0, Data, 18, 2);
            Buffer.BlockCopy(BitConverter.GetBytes((ushort)Path.Length + DisplayName.Length + 2), 0, Data, 20, 2);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(Path), 0, Data, 22, Path.Length * 2);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(DisplayName), 0, Data, 24 + Path.Length * 2, DisplayName.Length * 2);
            Buffer.BlockCopy(Encoding.Unicode.GetBytes(Comment), 0, Data, 26 + (Path.Length + DisplayName.Length) * 2, Comment.Length * 2);
            ItemIDList.Add(new ItemID(Data));
        }
        #endregion // Constructor
    }
}
