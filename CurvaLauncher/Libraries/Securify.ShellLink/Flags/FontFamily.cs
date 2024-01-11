using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// FontFamily (4 bytes): A 32-bit, unsigned integer that specifies the family of the 
    /// font used in the console window.
    /// </summary>
    public enum FontFamily : uint
    {
        /// <summary>
        /// The font family is unknown.
        /// </summary>
        FF_DONTCARE = 0x0000,
        /// <summary>
        /// The font is variable-width with serifs; for example, "Times New Roman".
        /// </summary>
        FF_ROMAN = 0x0010,
        /// <summary>
        /// The font is variable-width without serifs; for example, "Arial".
        /// </summary>
        FF_SWISS = 0x0020,
        /// <summary>
        /// The font is fixed-width, with or without serifs; for example, "Courier New".
        /// </summary>
        FF_MODERN = 0x0030,
        /// <summary>
        /// The font is designed to look like handwriting; for example, "Cursive".
        /// </summary>
        FF_SCRIPT = 0x0040,
        /// <summary>
        /// The font is a novelty font; for example, "Old English".
        /// </summary>
        FF_DECORATIVE = 0x0050
    }
}