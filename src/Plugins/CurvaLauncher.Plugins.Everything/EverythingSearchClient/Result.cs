using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverythingSearchClient
{

	/// <summary>
	/// Read-only results of a search run
	/// </summary>
	public class Result
	{

		/// <summary>
		/// Descriptive type flags for items
		/// </summary>
		[Flags]
		public enum ItemFlags
		{
			/// <summary>
			/// Item is a normal file
			/// </summary>
			None = 0,

			/// <summary>
			/// Item is a folder
			/// </summary>
			Folder = 1,

			/// <summary>
			/// Item is a drive or root
			/// </summary>
			Drive = 2,

			/// <summary>
			/// Something strange
			/// </summary>
			Unknown = 0x80

		}

		/// <summary>
		/// File attribute flags
		/// </summary>
		[Flags]
		public enum ItemFileAttributes
		{
			/// <summary>
			/// No file attributes are set
			/// </summary>
			None = 0,

			/// <summary>
			/// The file is read-only.
			/// </summary>
			ReadOnly = 1,

			/// <summary>
			/// The file is hidden, and thus is not included in an ordinary directory listing.
			/// </summary>
			Hidden = 2,

			/// <summary>
			/// The file is a system file.
			/// </summary>
			System = 4,

			/// <summary>
			/// The file is a directory.
			/// </summary>
			Directory = 16,

			/// <summary>
			/// This file is marked to be included in incremental backup operation.
			/// </summary>
			Archive = 32,

			/// <summary>
			/// The file is a standard file that has no special attributes.
			/// This attribute is valid only if it is used alone.
			/// </summary>
			Normal = 128

		}

		/// <summary>
		/// Description of one item
		/// </summary>
		public class Item
		{

			/// <summary>
			/// Gets the type flags
			/// </summary>
			public ItemFlags Flags { get; protected set; } = ItemFlags.None;

			/// <summary>
			/// Gets the name of the item, not including any path segment
			/// </summary>
			public string Name { get; protected set; } = string.Empty;

			/// <summary>
			/// Gets the path to the item, without the item's name
			/// </summary>
			public string Path { get; protected set; } = string.Empty;

			/// <summary>
			/// Gets the optional size of the file on disk in bytes
			/// </summary>
			public ulong? Size { get; protected set; } = null;

			/// <summary>
			/// Gets the optional creation date time
			/// </summary>
			public DateTime? CreationTime{ get; protected set; } = null;

			/// <summary>
			/// Gets the optional date time of the last file modification
			/// </summary>
			public DateTime? LastWriteTime { get; protected set; } = null;

			/// <summary>
			/// Gets the optional file attributes
			/// </summary>
			public ItemFileAttributes? FileAttributes { get; protected set; } = null;

		}

		/// <summary>
		/// Gets the total number of items found in the search run
		/// </summary>
		public UInt32 TotalItems { get; protected set; } = 0;

		/// <summary>
		/// Gets the number of items returned in this object
		/// </summary>
		public UInt32 NumItems { get => (UInt32)Items.Length; }

		/// <summary>
		/// Gets the offset of the results array of this object
		/// inside the total results list of the search run.
		/// </summary>
		public UInt32 Offset { get; protected set; } = 0;

		/// <summary>
		/// Gets the array of found items
		/// </summary>
		public Item[] Items {get; protected set; } = Array.Empty<Item>();

	}

}
