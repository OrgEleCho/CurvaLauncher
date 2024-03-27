using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EverythingSearchClient
{

	/// <summary>
	/// This class is based on `everything_ipc.h` from the Everything SDK
	/// </summary>
	internal class EverythingIPC
	{

		/// <summary>
		/// find the everything IPC window
		/// </summary>
		internal const string EVERYTHING_IPC_WNDCLASS = "EVERYTHING_TASKBAR_NOTIFICATION";

		/// <summary>
		/// Main message to contact Everything IPC
		/// </summary>
		internal const uint EVERYTHING_WM_IPC = 0x0400; // (WM_USER)

		/// <summary>
		/// int major_version = (int)SendMessage(everything_taskbar_notification_hwnd,EVERYTHING_WM_IPC,EVERYTHING_IPC_GET_MAJOR_VERSION,0);
		/// </summary>
		internal const uint EVERYTHING_IPC_GET_MAJOR_VERSION = 0;

		/// <summary>
		/// int minor_version = (int)SendMessage(everything_taskbar_notification_hwnd,EVERYTHING_WM_IPC,EVERYTHING_IPC_GET_MINOR_VERSION,0);
		/// </summary>
		internal const uint EVERYTHING_IPC_GET_MINOR_VERSION = 1;

		/// <summary>
		/// int revision = (int)SendMessage(everything_taskbar_notification_hwnd,EVERYTHING_WM_IPC,EVERYTHING_IPC_GET_REVISION,0);
		/// </summary>
		internal const uint EVERYTHING_IPC_GET_REVISION = 2;

		/// <summary>
		/// int build = (int)SendMessage(everything_taskbar_notification_hwnd,EVERYTHING_WM_IPC,EVERYTHING_IPC_GET_BUILD,0);
		/// </summary>
		internal const uint EVERYTHING_IPC_GET_BUILD_NUMBER = 3;

		/// <summary>
		/// int is_db_busy = (int)SendMessage(everything_taskbar_notification_hwnd,EVERYTHING_WM_IPC,EVERYTHING_IPC_IS_DB_BUSY,0);
		/// db is busy, issueing another action will cancel the current one (if possible).
		/// </summary>
		internal const uint EVERYTHING_IPC_IS_DB_BUSY = 402;

		[StructLayout(LayoutKind.Sequential)]
		internal struct EVERYTHING_IPC_QUERY
		{
			// the window that will receive the new results.
			// only 32bits are required to store a window handle. (even on x64)
			public UInt32 reply_hwnd;

			// the value to set the dwData member in the COPYDATASTRUCT struct 
			// sent by Everything when the query is complete.
			public UInt32 reply_copydata_message;

			// search flags (see EVERYTHING_IPC_MATCHCASE | EVERYTHING_IPC_MATCHWHOLEWORD | EVERYTHING_IPC_MATCHPATH)
			public UInt32 search_flags;

			// only return results after 'offset' results (0 to return from the first result)
			// useful for scrollable lists
			public UInt32 offset;

			// the number of results to return 
			// zero to return no results
			// EVERYTHING_IPC_ALLRESULTS to return ALL results
			public UInt32 max_results;

			// followed by null terminated wide string. variable lengthed search string buffer.
		}

		/// <summary>
		/// match case
		/// </summary>
		internal const uint EVERYTHING_IPC_MATCHCASE = 0x00000001;

		/// <summary>
		/// match whole word
		/// </summary>
		internal const uint EVERYTHING_IPC_MATCHWHOLEWORD = 0x00000002;

		/// <summary>
		/// include paths in search
		/// </summary>
		internal const uint EVERYTHING_IPC_MATCHPATH = 0x00000004;

		/// <summary>
		/// enable regex
		/// </summary>
		internal const uint EVERYTHING_IPC_REGEX = 0x00000008;

		/// <summary>
		/// the WM_COPYDATA message for a query.
		/// Use the unicode UTF16 variant
		/// </summary>
		internal const uint EVERYTHING_IPC_COPYDATAQUERYW = 2;

		/// <summary>
		/// the WM_COPYDATA message for a query 2.
		/// Use the unicode UTF16 variant
		/// </summary>
		internal const uint EVERYTHING_IPC_COPYDATA_QUERY2W = 18;

		/// <summary>
		/// The item is a folder. (it's a file if not set)
		/// </summary>
		internal const uint EVERYTHING_IPC_FOLDER = 0x00000001;

		/// <summary>
		/// the file or folder is a drive/root.
		/// </summary>
		internal const uint EVERYTHING_IPC_DRIVE = 0x00000002;

		[StructLayout(LayoutKind.Sequential)]
		internal struct EVERYTHING_IPC_QUERY2
		{
			// the window that will receive the new results.
			// only 32bits are required to store a window handle. (even on x64)
			public UInt32 reply_hwnd;

			// the value to set the dwData member in the COPYDATASTRUCT struct 
			// sent by Everything when the query is complete.
			public UInt32 reply_copydata_message;

			// search flags (see EVERYTHING_IPC_MATCHCASE | EVERYTHING_IPC_MATCHWHOLEWORD | EVERYTHING_IPC_MATCHPATH)
			public UInt32 search_flags;

			// only return results after 'offset' results (0 to return from the first result)
			// useful for scrollable lists
			public UInt32 offset;

			// the number of results to return 
			// zero to return no results
			// EVERYTHING_IPC_ALLRESULTS to return ALL results
			public UInt32 max_results;

			// request types.
			// one or more of EVERYTHING_IPC_QUERY2_REQUEST_* types.
			public UInt32 request_flags;

			// sort type, set to one of EVERYTHING_IPC_SORT_* types.
			// set to EVERYTHING_IPC_SORT_NAME_ASCENDING for the best performance (there will never be a performance hit when sorting by name ascending).
			// Other sorts will also be instant if the corresponding fast sort is enabled from Tools -> Options -> Indexes.
			public UInt32 sort_type;

			// followed by null terminated search.
		}

		/// <summary>
		/// Request name string
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_NAME = 0x00000001;

		/// <summary>
		/// Request path string
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_PATH = 0x00000002;

		/// <summary>
		/// Request path-and-name string
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_FULL_PATH_AND_NAME = 0x00000004;

		/// <summary>
		/// Request file name extension string
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_EXTENSION = 0x00000008;

		/// <summary>
		/// Request file size
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_SIZE = 0x00000010;

		/// <summary>
		/// Request file creation date
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_DATE_CREATED = 0x00000020;

		/// <summary>
		/// Request file modified date
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_DATE_MODIFIED = 0x00000040;

		/// <summary>
		/// Request file attributes
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_DATE_ACCESSED = 0x00000080;

		/// <summary>
		/// Request file attributes
		/// </summary>
		internal const uint EVERYTHING_IPC_QUERY2_REQUEST_ATTRIBUTES = 0x00000100;

		/// <summary>
		/// Sort results by file name ascending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_NAME_ASCENDING = 1;

		/// <summary>
		/// Sort results by file name name decending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_NAME_DESCENDING = 2;

		/// <summary>
		/// Sort results by full path ascending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_PATH_ASCENDING = 3;

		/// <summary>
		/// Sort results by full path decending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_PATH_DESCENDING = 4;

		/// <summary>
		/// Sort results by file size ascending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_SIZE_ASCENDING = 5;

		/// <summary>
		/// Sort results by file size decending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_SIZE_DESCENDING = 6;

		/// <summary>
		/// Sort results by extension ascending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_EXTENSION_ASCENDING = 7;

		/// <summary>
		/// Sort results by extension decending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_EXTENSION_DESCENDING = 8;

		/// <summary>
		/// Sort results by creation date ascending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_DATE_CREATED_ASCENDING = 11;

		/// <summary>
		/// Sort results by creation date decending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_DATE_CREATED_DESCENDING = 12;

		/// <summary>
		/// Sort results by date of last modification ascending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_DATE_MODIFIED_ASCENDING = 13;

		/// <summary>
		/// Sort results by date of last modification decending
		/// </summary>
		internal const uint EVERYTHING_IPC_SORT_DATE_MODIFIED_DESCENDING = 14;

	}

}
