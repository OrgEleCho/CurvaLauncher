using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// LinkInfoFlags (4 bytes): Flags that specify whether the VolumeID, LocalBasePath, LocalBasePathUnicode, 
    /// and CommonNetworkRelativeLink fields are present in this structure.
    /// </summary>
    [Flags]
    public enum LinkInfoFlags : uint
    {
        /// <summary>
        /// If set, the VolumeID and LocalBasePath fields are present, and their locations are specified by 
        /// the values of the VolumeIDOffset and LocalBasePathOffset fields, respectively. If the value of the 
        /// LinkInfoHeaderSize field is greater than or equal to 0x00000024, the LocalBasePathUnicode field is 
        /// present, and its location is specified by the value of the LocalBasePathOffsetUnicode field.
        ///
        /// If not set, the VolumeID, LocalBasePath, and LocalBasePathUnicode fields are not present, and the 
        /// values of the VolumeIDOffset and LocalBasePathOffset fields are zero. If the value of the 
        /// LinkInfoHeaderSize field is greater than or equal to 0x00000024, the value of the 
        /// LocalBasePathOffsetUnicode field is zero.
        /// </summary>
        VolumeIDAndLocalBasePath = 0x1,
        /// <summary>
        /// If set, the CommonNetworkRelativeLink field is present, and its location is specified by the value 
        /// of the CommonNetworkRelativeLinkOffset field.
        /// 
        /// If not set, the CommonNetworkRelativeLink field is not present, and the value of the 
        /// CommonNetworkRelativeLinkOffset field is zero.
        /// </summary>
        CommonNetworkRelativeLinkAndPathSuffix = 0x2
    }
}
