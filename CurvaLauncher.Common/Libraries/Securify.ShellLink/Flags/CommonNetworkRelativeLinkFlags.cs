using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// CommonNetworkRelativeLinkFlags (4 bytes): Flags that specify the contents of the 
    /// DeviceNameOffset and NetProviderType fields.
    /// </summary>
    [Flags]
    public enum CommonNetworkRelativeLinkFlags : uint
    {
        /// <summary>
        /// If set, the DeviceNameOffset field contains an offset to the device name.
        /// 
        /// If not set, the DeviceNameOffset field does not contain an offset to the device name, 
        /// and its value MUST be zero.
        /// </summary>
        ValidDevice = 0x1,
        /// <summary>
        /// If set, the NetProviderType field contains the network provider type.
        /// 
        /// If not set, the NetProviderType field does not contain the network provider type, 
        /// and its value MUST be zero.
        /// </summary>
        ValidNetType = 0x2
    }
}
