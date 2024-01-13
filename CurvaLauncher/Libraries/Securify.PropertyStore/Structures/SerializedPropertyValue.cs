using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Structures
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SerializedPropertyValue : Structure
    {
        /// <summary>
        /// An unsigned integer that specifies the total size, in bytes, of this structure.
        /// It MUST be 0x00000000 if this is the last The Serialized Property Value in the 
        /// enclosing Serialized Property Storage structure.
        /// </summary>
        public virtual uint ValueSize => 9;

        /// <summary>
        /// Value (variable): A TypedPropertyValue structure, as specified in [MS-OLEPS] section 2.15
        /// </summary>
        public TypedPropertyValue? TypedPropertyValue { get; set; }
    }
}
