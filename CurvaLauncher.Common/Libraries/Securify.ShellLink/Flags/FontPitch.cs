using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// FontFamily (4 bytes): A 32-bit, unsigned integer that specifies the family of the 
    /// font used in the console window.
    /// </summary>
    public enum FontPitch : uint
    {
        /// <summary>
        /// A font pitch does not apply.
        /// </summary>
        TMPF_NONE = 0x0000,
        /// <summary>
        /// The font is a fixed-pitch font.
        /// </summary>
        TMPF_FIXED_PITCH = 0x0001,
        /// <summary>
        /// The font is a vector font.
        /// </summary>
        TMPF_VECTOR = 0x0002,
        /// <summary>
        /// The font is a true-type font.
        /// </summary>
        TMPF_TRUETYPE = 0x0004,
        /// <summary>
        /// The font is specific to the device.
        /// </summary>
        TMPF_DEVICE = 0x0008
    }
}
