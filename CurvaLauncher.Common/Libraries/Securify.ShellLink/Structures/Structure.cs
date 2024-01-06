using System;
using System.Text;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Structures
{
    /// <summary>
    /// Abstract Structure class
    /// </summary>
    public abstract class Structure
    {
        /// <summary>
        /// Convert the Structure to a byte array
        /// </summary>
        /// <returns>Byte array representation of the Structure</returns>
        public abstract byte[] GetBytes();

        /// <inheritdoc />
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}:", GetType().Name);
            builder.AppendLine();
            return builder.ToString();
        }
    }
}
