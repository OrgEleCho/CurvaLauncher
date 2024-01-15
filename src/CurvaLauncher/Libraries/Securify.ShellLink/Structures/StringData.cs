using System;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// StringData refers to a set of structures that convey user interface and path identification information. 
    /// The presence of these optional structures is controlled by LinkFlags in the ShellLinkHeader.
    /// </summary>
    public class StringData : Structure
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public StringData() : this(true) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="IsUnicode">Indicates whether wide chars should be used</param>
        public StringData(bool IsUnicode) : base() { this.IsUnicode = IsUnicode; }
        #endregion // Constructor

        /// <summary>
        /// When set to true, wide chars will be used
        /// </summary>
        public bool IsUnicode { get; set; }

        /// <summary>
        /// NAME_STRING: An optional structure that specifies a description of the shortcut that is displayed to 
        /// end users to identify the purpose of the shell link. This structure MUST be present if the HasName 
        /// flag is set.
        /// </summary>
        public string? NameString { get; set; }

        /// <summary>
        /// RELATIVE_PATH: An optional structure that specifies the location of the link target relative to the 
        /// file that contains the shell link. When specified, this string SHOULD be used when resolving the link. 
        /// This structure MUST be present if the HasRelativePath flag is set.
        /// </summary>
        public string? RelativePath { get; set; }

        /// <summary>
        /// WORKING_DIR: An optional structure that specifies the file system path of the working directory to be 
        /// used when activating the link target. This structure MUST be present if the HasWorkingDir flag is set.
        /// </summary>
        public string? WorkingDir { get; set; }

        /// <summary>
        /// COMMAND_LINE_ARGUMENTS: An optional structure that stores the command-line arguments that are specified 
        /// when activating the link target. This structure MUST be present if the HasArguments flag is set.
        /// </summary>
        public string? CommandLineArguments { get; set; }

        /// <summary>
        /// ICON_LOCATION: An optional structure that specifies the location of the icon to be used when displaying a 
        /// shell link item in an icon view.This structure MUST be present if the HasIconLocation flag is set.
        /// </summary>
        public string? IconLocation { get; set; }

        #region StringDataSize
        /// <summary>
        /// Total size in bytes of the StringData structure
        /// </summary>
        public int StringDataSize
        {
            get
            {
                int Size = 0;
                if (NameString != null)
                {
                    Size += (IsUnicode ? (NameString.Length + 1) * 2 : NameString.Length + 1) + 2;
                }
                if (RelativePath != null)
                {
                    Size += (IsUnicode ? (RelativePath.Length + 1) * 2 : RelativePath.Length + 1) + 2;
                }
                if (WorkingDir != null)
                {
                    Size += (IsUnicode ? (WorkingDir.Length + 1) * 2 : WorkingDir.Length + 1) + 2;
                }
                if (CommandLineArguments != null)
                {
                    Size += (IsUnicode ? (CommandLineArguments.Length + 1) * 2 : CommandLineArguments.Length + 1) + 2;
                }
                if (IconLocation != null)
                {
                    Size += (IsUnicode ? (IconLocation.Length + 1) * 2 : IconLocation.Length + 1) + 2;
                }
                return Size;
            }
        }
        #endregion // StringDataSize

        #region GetBytes
        /// <inheritdoc />
        public override byte[] GetBytes()
        {
            ushort Count = 0;
            int Offset = 0;
            byte[] StringData = new byte[StringDataSize];
            if (NameString != null)
            {
                Count = (ushort)NameString.Length;
                Buffer.BlockCopy(BitConverter.GetBytes(Count + 1), 0, StringData, Offset, 2);
                if (IsUnicode)
                {
                    Buffer.BlockCopy(Encoding.Unicode.GetBytes(NameString), 0, StringData, Offset + 2, Count * 2);
                    Offset += Count * 2 + 4;

                }
                else
                {
                    Buffer.BlockCopy(Encoding.Default.GetBytes(NameString), 0, StringData, Offset + 2, Count);
                    Offset += Count + 3;
                }
            }
            if (RelativePath != null)
            {
                Count = (ushort)RelativePath.Length;
                Buffer.BlockCopy(BitConverter.GetBytes(Count + 1), 0, StringData, Offset, 2);
                if (IsUnicode)
                {
                    Buffer.BlockCopy(Encoding.Unicode.GetBytes(RelativePath), 0, StringData, Offset + 2, Count * 2);
                    Offset += Count * 2 + 4;
                }
                else
                {
                    Buffer.BlockCopy(Encoding.Default.GetBytes(RelativePath), 0, StringData, Offset + 2, Count);
                    Offset += Count + 3;
                }
            }
            if (WorkingDir != null)
            {
                Count = (ushort)WorkingDir.Length;
                Buffer.BlockCopy(BitConverter.GetBytes(Count + 1), 0, StringData, Offset, 2);
                if (IsUnicode)
                {
                    Buffer.BlockCopy(Encoding.Unicode.GetBytes(WorkingDir), 0, StringData, Offset + 2, Count * 2);
                    Offset += Count * 2 + 4;
                }
                else
                {
                    Buffer.BlockCopy(Encoding.Default.GetBytes(WorkingDir), 0, StringData, Offset + 2, Count);
                    Offset += Count + 3;
                }
            }
            if (CommandLineArguments != null)
            {
                Count = (ushort)CommandLineArguments.Length;
                Buffer.BlockCopy(BitConverter.GetBytes(Count + 1), 0, StringData, Offset, 2);
                if (IsUnicode)
                {
                    Buffer.BlockCopy(Encoding.Unicode.GetBytes(CommandLineArguments), 0, StringData, Offset + 2, Count * 2);
                    Offset += Count * 2 + 4;
                }
                else
                {
                    Buffer.BlockCopy(Encoding.Default.GetBytes(CommandLineArguments), 0, StringData, Offset + 2, Count);
                    Offset += Count + 3;
                }
            }
            if (IconLocation != null)
            {
                Count = (ushort)IconLocation.Length;
                Buffer.BlockCopy(BitConverter.GetBytes(Count + 1), 0, StringData, Offset, 2);
                if (IsUnicode)
                {
                    Buffer.BlockCopy(Encoding.Unicode.GetBytes(IconLocation), 0, StringData, Offset + 2, Count * 2);
                    Offset += Count * 2 + 4;
                }
                else
                {
                    Buffer.BlockCopy(Encoding.Default.GetBytes(IconLocation), 0, StringData, Offset + 2, Count);
                    Offset += Count + 3;
                }
            }
            return StringData;
        }
        #endregion // GetBytes

        #region ToString
        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(base.ToString());
            if (NameString != null)
            {
                builder.AppendFormat("NameString: {0}", NameString);
                builder.AppendLine();
            }
            if (RelativePath != null)
            {
                builder.AppendFormat("RelativePath: {0}", RelativePath);
                builder.AppendLine();
            }
            if (WorkingDir != null)
            {
                builder.AppendFormat("WorkingDir: {0}", WorkingDir);
                builder.AppendLine();
            }
            if (CommandLineArguments != null)
            {
                builder.AppendFormat("CommandLineArguments: {0}", CommandLineArguments);
                builder.AppendLine();
            }
            if (IconLocation != null)
            {
                builder.AppendFormat("IconLocation: {0}", IconLocation);
                builder.AppendLine();
            }
            return builder.ToString();
        }
        #endregion // ToString
    }
}
