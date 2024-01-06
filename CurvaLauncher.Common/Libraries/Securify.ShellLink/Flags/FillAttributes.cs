using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Flags
{
    /// <summary>
    /// FillAttributes (2 bytes): A 16-bit, unsigned integer that specifies the fill 
    /// attributes that control the foreground and background text colors in the console 
    /// window.
    /// </summary>
    [Flags]
    public enum FillAttributes : ushort
    {
        /// <summary>
        /// The foreground text color contains blue.
        /// </summary>
        FOREGROUND_BLUE = 0x0001,
        /// <summary>
        /// The foreground text color contains green.
        /// </summary>
        FOREGROUND_GREEN = 0x0002,
        /// <summary>
        /// The foreground text color contains red.
        /// </summary>
        FOREGROUND_RED = 0x0004,
        /// <summary>
        /// The foreground text color is intensified.
        /// </summary>
        FOREGROUND_INTENSITY = 0x0008,
        /// <summary>
        /// The background text color contains blue.
        /// </summary>
        BACKGROUND_BLUE = 0x0010,
        /// <summary>
        /// The background text color contains green.
        /// </summary>
        BACKGROUND_GREEN = 0x0020,
        /// <summary>
        /// The background text color contains red.
        /// </summary>
        BACKGROUND_RED = 0x0040,
        /// <summary>
        /// The background text color is intensified.
        /// </summary>
        BACKGROUND_INTENSITY = 0x0080
    }
}
