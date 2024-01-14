using System;

namespace CurvaLauncher.Libraries.Securify.ShellLink.Securify.PropertyStore.Flags
{
    /// <summary>
    /// The PropertyType enumeration represents the type of a property in a property set. The set of types supported depends 
    /// on the version of the property set, which is indicated by the Version field of the PropertySetStream packet.
    /// In addition, the property types not supported in simple property sets are specified as such. 
    /// </summary>
    [Flags]
    public enum PropertyType : ushort
    {
        /// <summary>
        /// Type is undefined, and the minimum property set version is 0.
        /// </summary>
        VT_EMPTY = 0x0000,
        /// <summary>
        /// Type is null, and the minimum property set version is 0.
        /// </summary>
        VT_NULL = 0x0001,
        /// <summary>
        /// Type is 16-bit signed integer, and the minimum property set version is 0.
        /// </summary>
        VT_I2 = 0x0002,
        /// <summary>
        /// Type is 32-bit signed integer, and the minimum property set version is 0.
        /// </summary>
        VT_I4 = 0x0003,
        /// <summary>
        /// Type is 4-byte (single-precision) IEEE floating-point number, and the minimum property set version is 0.
        /// </summary>
        VT_R4 = 0x0004,
        /// <summary>
        /// Type is 8-byte (double-precision) IEEE floating-point number, and the minimum property set version is 0.
        /// </summary>
        VT_R8 = 0x0005,
        /// <summary>
        /// Type is CURRENCY, and the minimum property set version is 0.
        /// </summary>
        VT_CY = 0x0006,
        /// <summary>
        /// Type is DATE, and the minimum property set version is 0.
        /// </summary>
        VT_DATE = 0x0007,
        /// <summary>
        /// Type is CodePageString, and the minimum property set version is 0.
        /// </summary>
        VT_BSTR = 0x0008,
        /// <summary>
        /// Type is HRESULT, and the minimum property set version is 0.
        /// </summary>
        VT_ERROR = 0x000A,
        /// <summary>
        /// Type is VARIANT_BOOL, and the minimum property set version is 0.
        /// </summary>
        VT_BOOL = 0x000B,
        /// <summary>
        /// Type is DECIMAL, and the minimum property set version is 0.
        /// </summary>
        VT_DECIMAL = 0x000E,
        /// <summary>
        /// Type is 1-byte signed integer, and the minimum property set version is 1.
        /// </summary>
        VT_I1 = 0x0010,
        /// <summary>
        /// Type is 1-byte unsigned integer, and the minimum property set version is 0.
        /// </summary>
        VT_UI1 = 0x0011,
        /// <summary>
        /// Type is 2-byte unsigned integer, and the minimum property set version is 0.
        /// </summary>
        VT_UI2 = 0x0012,
        /// <summary>
        /// Type is 4-byte unsigned integer, and the minimum property set version is 0.
        /// </summary>
        VT_UI4 = 0x0013,
        /// <summary>
        /// Type is 8-byte signed integer, and the minimum property set version is 0.
        /// </summary>
        VT_I8 = 0x0014,
        /// <summary>
        /// Type is 8-byte unsigned integer, and the minimum property set version is 0.
        /// </summary>
        VT_UI8 = 0x0015,
        /// <summary>
        /// Type is 4-byte signed integer, and the minimum property set version is 1.
        /// </summary>
        VT_INT = 0x0016,
        /// <summary>
        /// Type is 4-byte unsigned integer, and the minimum property set version is 1.
        /// </summary>
        VT_UINT = 0x0017,
        /// <summary>
        /// Type is CodePageString, and the minimum property set version is 0.
        /// </summary>
        VT_LPSTR = 0x001E,
        /// <summary>
        /// Type is UnicodeString, and the minimum property set version is 0.
        /// </summary>
        VT_LPWSTR = 0x001F,
        /// <summary>
        /// Type is FILETIME, and the minimum property set version is 0.
        /// </summary>
        VT_FILETIME = 0x0040,
        /// <summary>
        /// Type is binary large object (BLOB), and the minimum property set version is 0.
        /// </summary>
        VT_BLOB = 0x0041,
        /// <summary>
        /// Type is Stream, and the minimum property set version is 0. VT_STREAM is not allowed in a simple property set.
        /// </summary>
        VT_STREAM = 0x0042,
        /// <summary>
        /// Type is Storage, and the minimum property set version is 0. VT_STORAGE is not allowed in a simple property set.
        /// </summary>
        VT_STORAGE = 0x0043,
        /// <summary>
        /// Type is Stream representing an Object in an application-specific manner, and the minimum property set version is 0. 
        /// VT_STREAMED_Object is not allowed in a simple property set.
        /// </summary>
        VT_STREAMED_Object = 0x0044,
        /// <summary>
        /// Type is Storage representing an Object in an application-specific manner, and the minimum property set version is 0. 
        /// VT_STORED_Object is not allowed in a simple property set.
        /// </summary>
        VT_STORED_Object = 0x0045,
        /// <summary>
        /// Type is BLOB representing an object in an application-specific manner. The minimum property set version is 0.
        /// </summary>
        VT_BLOB_Object = 0x0046,
        /// <summary>
        /// Type is PropertyIdentifier, and the minimum property set version is 0.
        /// </summary>
        VT_CF = 0x0047,
        /// <summary>
        /// Type is CLSID, and the minimum property set version is 0.
        /// </summary>
        VT_CLSID = 0x0048,
        /// <summary>
        /// Type is Stream with application-specific version GUID (VersionedStream). The minimum property set version is 0. 
        /// VT_VERSIONED_STREAM is not allowed in a simple property set.
        /// </summary>
        VT_VERSIONED_STREAM = 0x0049,
        /// <summary>
        /// Type is Vector
        /// </summary>
        VT_VECTOR = 0x1000,
        /// <summary>
        /// Type is Array
        /// </summary>
        VT_ARRAY = 0x2000
    }
}
