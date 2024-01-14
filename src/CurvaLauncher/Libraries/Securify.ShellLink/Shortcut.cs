using System;
using System.IO;
using System.Text;
using System.Linq;
using CurvaLauncher.Libraries.Securify.ShellLink.Flags;
using CurvaLauncher.Libraries.Securify.ShellLink.Structures;

namespace CurvaLauncher.Libraries.Securify.ShellLink
{
    /// <summary>
    /// The Shortcut class can be used to process &amp; create Shell Link Binary File Format files.
    /// In this format a structure is called a shell link, or shortcut, and is a data object 
    /// that contains information that can be used to access another data object. The Shell Link 
    /// Binary File Format is the format of Windows files with the extension "LNK".
    /// 
    /// Shell links are commonly used to support application launching and linking scenarios, 
    /// such as Object Linking and Embedding(OLE), but they also can be used by applications that 
    /// need the ability to store a reference to a target file.
    /// </summary>
    public class Shortcut : ShellLinkHeader
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Shortcut() : base()
        {
            ExtraData = new ExtraData();
        }
        #endregion // Constructor

        /// <summary>
        /// SHELL_LINK_HEADER: A ShellLinkHeader structure, which contains identification information, 
        /// timestamps, and flags that specify the presence of optional structures.
        /// </summary>
        public ShellLinkHeader ShellLinkHeader => this;

        #region LinkFlags
        /// <inheritdoc />
        public override LinkFlags LinkFlags
        {
            get
            {
                LinkFlags linkFlags = base.LinkFlags;

                if (LinkTargetIDList != null)
                {
                    linkFlags |= LinkFlags.HasLinkTargetIDList;
                }
                else
                {
                    linkFlags &= ~LinkFlags.HasLinkTargetIDList;
                }

                if (LinkInfo != null)
                {
                    linkFlags |= LinkFlags.HasLinkInfo;
                }
                else
                {
                    linkFlags &= ~LinkFlags.HasLinkInfo;
                }

                if (StringData != null)
                {
                    linkFlags &= ~(LinkFlags.HasName | LinkFlags.HasRelativePath | LinkFlags.HasWorkingDir | LinkFlags.HasArguments | LinkFlags.HasIconLocation);
                    if (StringData.NameString != null)
                    {
                        linkFlags |= LinkFlags.HasName;
                    }
                    if (StringData.RelativePath != null)
                    {
                        linkFlags |= LinkFlags.HasRelativePath;
                    }
                    if (StringData.WorkingDir != null)
                    {
                        linkFlags |= LinkFlags.HasWorkingDir;
                    }
                    if (StringData.CommandLineArguments != null)
                    {
                        linkFlags |= LinkFlags.HasArguments;
                    }
                    if (StringData.IconLocation != null)
                    {
                        linkFlags |= LinkFlags.HasIconLocation;
                    }
                    if (StringData.IsUnicode)
                    {
                        linkFlags |= LinkFlags.IsUnicode;
                    }
                }
                else
                {
                    linkFlags &= ~(LinkFlags.HasName | LinkFlags.HasRelativePath | LinkFlags.HasWorkingDir | LinkFlags.HasArguments | LinkFlags.HasIconLocation);
                }

                if (ExtraData.EnvironmentVariableDataBlock != null)
                {
                    linkFlags |= LinkFlags.HasExpString;
                }
                else
                {
                    linkFlags &= ~LinkFlags.HasExpString;
                }

                if (ExtraData.DarwinDataBlock != null)
                {
                    linkFlags |= LinkFlags.HasDarwinID;
                }
                else
                {
                    linkFlags &= ~LinkFlags.HasDarwinID;
                }

                if (ExtraData.IconEnvironmentDataBlock != null)
                {
                    linkFlags |= LinkFlags.HasExpIcon;
                }
                else
                {
                    linkFlags &= ~LinkFlags.HasExpIcon;
                }

                if (ExtraData.ShimDataBlock != null)
                {
                    linkFlags |= LinkFlags.RunWithShimLayer;
                }
                else
                {
                    linkFlags &= ~LinkFlags.RunWithShimLayer;
                }

                if (ExtraData.PropertyStoreDataBlock != null)
                {
                    linkFlags |= LinkFlags.EnableTargetMetadata;
                }

                return linkFlags;
            }
        }
        #endregion // LinkFlags

        /// <summary>
        /// LINKTARGET_IDLIST: An optional LinkTargetIDList structure, which specifies the target of 
        /// the link. The presence of this structure is specified by the HasLinkTargetIDList bit in the 
        /// ShellLinkHeader.
        /// </summary>
        public LinkTargetIDList? LinkTargetIDList { get; set; }

        /// <summary>
        /// LINKINFO: An optional LinkInfo structure, which specifies information necessary to resolve
        /// the link target. The presence of this structure is specified by the HasLinkInfo bit in the 
        /// ShellLinkHeader.
        /// </summary>
        public LinkInfo? LinkInfo { get; set; }

        /// <summary>
        /// STRING_DATA: Zero or more optional StringData structures, which are used to convey user 
        /// interface and path identification information. The presence of these structures is specified
        /// by bits in the ShellLinkHeader.
        /// </summary>
        public StringData? StringData { get; set; }

        /// <summary>
        /// EXTRA_DATA: Zero or more ExtraData structures.
        /// </summary>
        public ExtraData ExtraData { get; set; }

        #region ReadFromFile
        /// <summary>
        /// Reads a lnk and returns a Shortcut object
        /// </summary>
        /// <param name="path">Path to the lnk file</param>
        /// <returns>Shortcut object</returns>
        public static Shortcut ReadFromFile(string path)
        {
            return FromByteArray(File.ReadAllBytes(path));
        }

        #endregion // ReadFromFile

        #region CreateShortcut
        /// <summary>
        /// Create a new Shelllink (shortcut)
        /// </summary>
        /// <param name="path">Path the shortcut points to</param>
        /// <returns>Shortcut object</returns>
        public static Shortcut CreateShortcut(string path)
        {
            Shortcut lnk = new Shortcut();
            lnk.ExtraData.EnvironmentVariableDataBlock = new EnvironmentVariableDataBlock(path);
            return lnk;
        }

        /// <summary>
        /// Create a new Shelllink (shortcut)
        /// </summary>
        /// <param name="path">Path the shortcut points to</param>
        /// <param name="iconpath">Path to the file containing the icon</param>
        /// <param name="iconindex">Index ot the icon</param>
        /// <returns>Shortcut object</returns>
        public static Shortcut CreateShortcut(string path, string iconpath, int iconindex)
        {
            Shortcut lnk = CreateShortcut(path);
            lnk.IconIndex = iconindex;
            lnk.StringData = new StringData(true)
            {
                IconLocation = iconpath
            };
            lnk.ExtraData.IconEnvironmentDataBlock = new IconEnvironmentDataBlock(iconpath);
            return lnk;
        }

        /// <summary>
        /// Create a new Shelllink (shortcut)
        /// </summary>
        /// <param name="path">Path the shortcut points to</param>
        /// <param name="args">Command line arguments</param>
        /// <returns>Shortcut object</returns>
        public static Shortcut CreateShortcut(string path, string args)
        {
            Shortcut lnk = CreateShortcut(path);
            lnk.StringData = new StringData(true)
            {
                CommandLineArguments = args
            };
            return lnk;
        }

        /// <summary>
        /// Create a new Shelllink (shortcut)
        /// </summary>
        /// <param name="path">Path the shortcut points to</param>
        /// <param name="args">Command line arguments</param>
        /// <param name="iconpath">Path to the file containing the icon</param>
        /// <param name="iconindex">Index ot the icon</param>
        /// <returns>Shortcut object</returns>
        public static Shortcut CreateShortcut(string path, string args, string iconpath, int iconindex)
        {
            Shortcut lnk = CreateShortcut(path, args);
            lnk.IconIndex = iconindex;
            lnk.StringData!.IconLocation = iconpath;
            lnk.ExtraData.IconEnvironmentDataBlock = new IconEnvironmentDataBlock(iconpath);
            return lnk;
        }

        /// <summary>
        /// Create a new Shelllink (shortcut)
        /// </summary>
        /// <param name="path">Path the shortcut points to</param>
        /// <param name="args">Command line arguments</param>
        /// <param name="workdir">Working directory</param>
        /// <returns>Shortcut object</returns>
        public static Shortcut CreateShortcut(string path, string args, string workdir)
        {
            Shortcut lnk = CreateShortcut(path, args);
            lnk.StringData!.WorkingDir = workdir;
            return lnk;
        }

        /// <summary>
        /// Create a new Shelllink (shortcut)
        /// </summary>
        /// <param name="path">Path the shortcut points to</param>
        /// <param name="args">Command line arguments</param>
        /// <param name="workdir">Working directory</param>
        /// <param name="iconpath">Path to the file containing the icon</param>
        /// <param name="iconindex">Index ot the icon</param>
        /// <returns>Shortcut object</returns>
        public static Shortcut CreateShortcut(string path, string args, string workdir, string iconpath, int iconindex)
        {
            Shortcut lnk = CreateShortcut(path, args, iconpath, iconindex);
            lnk.StringData!.WorkingDir = workdir;
            return lnk;
        }
        #endregion // CreateShortcut

        #region WriteToFile
        /// <summary>
        /// Writes Shortcut object to a file
        /// </summary>
        /// <param name="path">Target lnk path</param>
        public void WriteToFile(string path)
        {
            File.WriteAllBytes(path, GetBytes());
        }
        #endregion // WriteToFile

        #region Size
        /// <summary>
        /// The size of the ShellLink in bytes
        /// </summary>
        public int Size
        {
            get
            {
                int size = (int)HeaderSize + ExtraData.ExtraDataSize;

                if ((LinkFlags & LinkFlags.HasLinkTargetIDList) != 0 && LinkTargetIDList != null)
                {
                    size += LinkTargetIDList.IDListSize + 2;
                }

                if ((LinkFlags & LinkFlags.HasLinkInfo) != 0 && LinkInfo != null)
                {
                    size += (int)LinkInfo.LinkInfoSize;
                }

                if (StringData != null)
                {
                    size += StringData.StringDataSize;
                }

                return size;
            }
        }
        #endregion // Size

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            int Offset = 0;
            byte[] lnk = new byte[Size];
            Buffer.BlockCopy(base.GetBytes(), 0, lnk, 0, (int)HeaderSize);
            Offset += (int)HeaderSize;

            if ((LinkFlags & LinkFlags.HasLinkTargetIDList) != 0 && LinkTargetIDList != null)
            {
                Buffer.BlockCopy(LinkTargetIDList.GetBytes(), 0, lnk, Offset, LinkTargetIDList.IDListSize + 2);
                Offset += LinkTargetIDList.IDListSize + 2;
            }

            if ((LinkFlags & LinkFlags.HasLinkInfo) != 0 && LinkInfo != null)
            {
                Buffer.BlockCopy(LinkInfo.GetBytes(), 0, lnk, Offset, (int)LinkInfo.LinkInfoSize);
                Offset += (int)LinkInfo.LinkInfoSize;
            }

            if (StringData != null)
            {
                Buffer.BlockCopy(StringData.GetBytes(), 0, lnk, Offset, StringData.StringDataSize);
                Offset += StringData.StringDataSize;
            }

            Buffer.BlockCopy(ExtraData.GetBytes(), 0, lnk, Offset, ExtraData.ExtraDataSize);
            return lnk;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());

            if ((LinkFlags & LinkFlags.HasLinkTargetIDList) != 0 && LinkTargetIDList != null)
            {
                builder.Append(LinkTargetIDList.ToString());
            }
            if ((LinkFlags & LinkFlags.HasLinkInfo) != 0 && LinkInfo != null)
            {
                builder.Append(LinkInfo.ToString());
            }
            if (StringData != null)
            {
                builder.Append(StringData.ToString());
            }
            if (ExtraData.ExtraDataSize > 4)
            {
                builder.Append(ExtraData.ToString());
            }
            return builder.ToString();
        }
        #endregion // ToString

        #region FromByteArray
        /// <summary>
        /// Create a ShellLing from a given byte array
        /// </summary>
        /// <param name="ba">The byte array</param>
        /// <returns>A Shortcut object</returns>
        public static new Shortcut FromByteArray(byte[] ba)
        {
            Shortcut lnk = new Shortcut();

            #region SHELL_LINK_HEADER
            ShellLinkHeader Header = ShellLinkHeader.FromByteArray(ba);
            uint HeaderSize = BitConverter.ToUInt32(ba, 0);
            bool IsUnicode = (Header.LinkFlags & LinkFlags.IsUnicode) != 0;
            lnk.LinkFlags = Header.LinkFlags;
            lnk.FileAttributes = Header.FileAttributes;
            lnk.CreationTime = Header.CreationTime;
            lnk.AccessTime = Header.AccessTime;
            lnk.WriteTime = Header.WriteTime;
            lnk.FileSize = Header.FileSize;
            lnk.IconIndex = Header.IconIndex;
            lnk.ShowCommand = Header.ShowCommand;
            lnk.HotKey = Header.HotKey;
            ba = ba.Skip((int)HeaderSize).ToArray();
            #endregion // SHELL_LINK_HEADER

            #region LINKTARGET_IDLIST
            if ((Header.LinkFlags & LinkFlags.HasLinkTargetIDList) != 0)
            {
                lnk.LinkTargetIDList = LinkTargetIDList.FromByteArray(ba);
                ushort IDListSize = BitConverter.ToUInt16(ba, 0);
                ba = ba.Skip(IDListSize + 2).ToArray();
            }
            #endregion // LINKTARGET_IDLIST

            #region LINKINFO
            if ((Header.LinkFlags & LinkFlags.HasLinkInfo) != 0)
            {
                lnk.LinkInfo = LinkInfo.FromByteArray(ba);
                uint LinkInfoSize = BitConverter.ToUInt32(ba, 0);
                ba = ba.Skip((int)LinkInfoSize).ToArray();
            }
            #endregion // LINKINFO

            #region STRING_DATA
            if ((Header.LinkFlags & LinkFlags.HasName) != 0)
            {
                if (lnk.StringData == null)
                {
                    lnk.StringData = new StringData(IsUnicode);
                }

                ushort CountCharacters = BitConverter.ToUInt16(ba, 0);
                if (IsUnicode)
                {
                    lnk.StringData.NameString = Encoding.Unicode.GetString(ba.Skip(2).Take(CountCharacters * 2).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters * 2 + 2).ToArray();
                }
                else
                {
                    lnk.StringData.NameString = Encoding.Default.GetString(ba.Skip(2).Take(CountCharacters).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters + 2).ToArray();
                }

            }

            if ((Header.LinkFlags & LinkFlags.HasRelativePath) != 0)
            {
                if (lnk.StringData == null)
                {
                    lnk.StringData = new StringData(IsUnicode);
                }

                ushort CountCharacters = BitConverter.ToUInt16(ba, 0);
                if (IsUnicode)
                {
                    lnk.StringData.RelativePath = Encoding.Unicode.GetString(ba.Skip(2).Take(CountCharacters * 2).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters * 2 + 2).ToArray();
                }
                else
                {
                    lnk.StringData.RelativePath = Encoding.Default.GetString(ba.Skip(2).Take(CountCharacters).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters + 2).ToArray();
                }
            }

            if ((Header.LinkFlags & LinkFlags.HasWorkingDir) != 0)
            {
                if (lnk.StringData == null)
                {
                    lnk.StringData = new StringData(IsUnicode);
                }

                ushort CountCharacters = BitConverter.ToUInt16(ba, 0);
                if (IsUnicode)
                {
                    lnk.StringData.WorkingDir = Encoding.Unicode.GetString(ba.Skip(2).Take(CountCharacters * 2).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters * 2 + 2).ToArray();
                }
                else
                {
                    lnk.StringData.WorkingDir = Encoding.Default.GetString(ba.Skip(2).Take(CountCharacters).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters + 2).ToArray();
                }
            }

            if ((Header.LinkFlags & LinkFlags.HasArguments) != 0)
            {
                if (lnk.StringData == null)
                {
                    lnk.StringData = new StringData(IsUnicode);
                }

                ushort CountCharacters = BitConverter.ToUInt16(ba, 0);
                if (IsUnicode)
                {
                    lnk.StringData.CommandLineArguments = Encoding.Unicode.GetString(ba.Skip(2).Take(CountCharacters * 2).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters * 2 + 2).ToArray();
                }
                else
                {
                    lnk.StringData.CommandLineArguments = Encoding.Default.GetString(ba.Skip(2).Take(CountCharacters).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters + 2).ToArray();
                }
            }

            if ((Header.LinkFlags & LinkFlags.HasIconLocation) != 0)
            {
                if (lnk.StringData == null)
                {
                    lnk.StringData = new StringData(IsUnicode);
                }

                ushort CountCharacters = BitConverter.ToUInt16(ba, 0);
                if (IsUnicode)
                {
                    lnk.StringData.IconLocation = Encoding.Unicode.GetString(ba.Skip(2).Take(CountCharacters * 2).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters * 2 + 2).ToArray();
                }
                else
                {
                    lnk.StringData.IconLocation = Encoding.Default.GetString(ba.Skip(2).Take(CountCharacters).ToArray()).TrimEnd(new char[] { (char)0 });
                    ba = ba.Skip(CountCharacters + 2).ToArray();
                }
            }
            #endregion // STRING_DATA

            #region EXTRA_DATA
            if (ba.Length >= 4)
            {
                lnk.ExtraData = ExtraData.FromByteArray(ba);
            }
            #endregion // EXTRA_DATA

            return lnk;
        }
        #endregion // FromByteArray
    }
}