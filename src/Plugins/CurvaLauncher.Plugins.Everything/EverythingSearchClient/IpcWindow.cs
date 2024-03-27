using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace EverythingSearchClient
{

	/// <summary>
	/// Manages the detected IpcWindow of Everything
	/// </summary>
	[SupportedOSPlatform("windows")]
	internal class IpcWindow
	{

		public IntPtr HWnd { get; private set; } = IntPtr.Zero;

		public IpcWindow()
		{
			Detect();
		}

		public void Detect()
		{
			HWnd = FindWindow(EverythingIPC.EVERYTHING_IPC_WNDCLASS, null);
		}

		public bool IsAvailable
		{
			get
			{
				return HWnd != IntPtr.Zero;
			}
		}

		public bool IsBusy()
		{
			uint b = SendMessage(HWnd, EverythingIPC.EVERYTHING_WM_IPC, EverythingIPC.EVERYTHING_IPC_IS_DB_BUSY, 0);
			return b != 0;
		}

		public Version GetVersion()
		{
			int ma = (int)SendMessage(HWnd, EverythingIPC.EVERYTHING_WM_IPC, EverythingIPC.EVERYTHING_IPC_GET_MAJOR_VERSION, 0);
			int mi = (int)SendMessage(HWnd, EverythingIPC.EVERYTHING_WM_IPC, EverythingIPC.EVERYTHING_IPC_GET_MINOR_VERSION, 0);
			int re = (int)SendMessage(HWnd, EverythingIPC.EVERYTHING_WM_IPC, EverythingIPC.EVERYTHING_IPC_GET_REVISION, 0);
			int bu = (int)SendMessage(HWnd, EverythingIPC.EVERYTHING_WM_IPC, EverythingIPC.EVERYTHING_IPC_GET_BUILD_NUMBER, 0);
			// funny, how "revision" and "build" are order arbitrarily by different people/organizations
			return new Version(ma, mi, re, bu);
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string lpClassName, string? lpWindowName);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern uint SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

	}

}
