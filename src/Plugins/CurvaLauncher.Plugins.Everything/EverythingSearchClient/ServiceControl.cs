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
	/// Utility class to control the Everything service, if installed
	/// </summary>
	[SupportedOSPlatform("windows")]
	public static class ServiceControl
	{

		/// <summary>
		/// Checks whether or not the Everything service is installed.
		/// </summary>
		/// <remarks>
		/// If
		///    SearchClient.IsEverythingAvailable() == true
		/// and
		///    ServiceControl.IsServiceInstalled() == false
		///  or
		///    ServiceControl.IsServiceRunning() == false
		/// then, the everything functions are served by a non-daemon process started by the user.
		/// </remarks>
		public static bool IsServiceInstalled()
		{
			bool isInstalled = false;

			(IntPtr hSCM, IntPtr hService) = OpenService(false);

			if (hService != IntPtr.Zero)
			{
				isInstalled = true;
				CloseServiceHandle(hService);
			}
			CloseServiceHandle(hSCM);

			return isInstalled;
		}

		/// <summary>
		/// Checks whether or not the Everything service is installed and running.
		/// </summary>
		/// <remarks>
		/// If
		///    SearchClient.IsEverythingAvailable() == true
		/// and
		///    ServiceControl.IsServiceInstalled() == false
		///  or
		///    ServiceControl.IsServiceRunning() == false
		/// then, the everything functions are served by a non-daemon process started by the user.
		/// </remarks>
		public static bool IsServiceRunning()
		{
			bool isRunning = false;

			(IntPtr hSCM, IntPtr hService) = OpenService(false);

			if (hService != IntPtr.Zero)
			{
				isRunning = QueryIsServiceRunning(hService);
				CloseServiceHandle(hService);
			}

			CloseServiceHandle(hSCM);

			return isRunning;
		}

		/// <summary>
		/// Starts the Everything service if stopped.
		/// </summary>
		/// <exception cref="InvalidOperationException">If the Everything service is not installed.</exception>
		/// <exception cref="UnauthorizedAccessException">Cannot change service state, because OS denied access.
		/// You likely need to run the process calling this function with elevated user rights.</exception>
		public static void Start()
		{
			(IntPtr hSCM, IntPtr hService) = OpenService(true);
			try
			{
				if (hService == IntPtr.Zero) throw new InvalidOperationException("Service is not installed");
				bool isRunning = QueryIsServiceRunning(hService);
				if (isRunning) return;

				if (!StartService(hService, 0, Array.Empty<string>()))
				{
					int lastError = Marshal.GetLastPInvokeError();
					if (lastError == ERROR_ACCESS_DENIED) throw new UnauthorizedAccessException($"Cannot start service: {lastError}");
					throw new Exception($"Cannot start service: {lastError}");
				}
			}
			finally
			{
				if (hService != IntPtr.Zero) CloseServiceHandle(hService);
				if (hSCM != IntPtr.Zero) CloseServiceHandle(hSCM);
			}
		}

		/// <summary>
		/// Stops the Everything service if running.
		/// </summary>
		/// <exception cref="InvalidOperationException">If the Everything service is not installed.</exception>
		/// <exception cref="UnauthorizedAccessException">Cannot change service state, because OS denied access.
		/// You likely need to run the process calling this function with elevated user rights.</exception>
		public static void Stop()
		{
			(IntPtr hSCM, IntPtr hService) = OpenService(true);
			try
			{
				if (hService == IntPtr.Zero) throw new InvalidOperationException("Service is not installed");
				bool isRunning = QueryIsServiceRunning(hService);
				if (!isRunning) return;

				SERVICE_STATUS status = new SERVICE_STATUS();
				if (!ControlService(hService, SERVICE_CONTROL_STOP, ref status))
				{
					int lastError = Marshal.GetLastPInvokeError();
					if (lastError == ERROR_ACCESS_DENIED) throw new UnauthorizedAccessException($"Cannot stop service: {lastError}");
					throw new Exception($"Cannot stop service: {lastError}");
				}

			}
			finally
			{
				if (hService != IntPtr.Zero) CloseServiceHandle(hService);
				if (hSCM != IntPtr.Zero) CloseServiceHandle(hSCM);
			}
		}

		private static (IntPtr, IntPtr) OpenService(bool change)
		{
			IntPtr hSCM = OpenSCManager(null, null, SC_MANAGER_CONNECT);
			if (hSCM == IntPtr.Zero) throw new Exception("Cannot access service control manager");

			IntPtr hService = OpenService(hSCM,
				"Everything",
				change
				? (SERVICE_START | SERVICE_STOP | SERVICE_QUERY_STATUS)
				: SERVICE_QUERY_STATUS);
			if (hService == IntPtr.Zero)
			{
				uint lastError = (uint)Marshal.GetLastPInvokeError();
				if (ERROR_SERVICE_DOES_NOT_EXIST != lastError)
				{
					CloseServiceHandle(hSCM);
					throw new Exception($"Cannot open service handle: {lastError}");
				}
			}

			return (hSCM, hService);
		}

		private static bool QueryIsServiceRunning(IntPtr hService)
		{
			SERVICE_STATUS_PROCESS status = new SERVICE_STATUS_PROCESS();
			if (QueryServiceStatusEx(hService, SC_STATUS_PROCESS_INFO, ref status, Marshal.SizeOf<SERVICE_STATUS_PROCESS>(), out _))
			{
				return (status.currentState & SERVICE_RUNNING) == SERVICE_RUNNING;
			}
			return false;
		}

		private const uint SC_MANAGER_CONNECT = 0x0001;
		private const uint SERVICE_QUERY_STATUS = 0x0004;
		private const uint SERVICE_START = 0x0010;
		private const uint SERVICE_STOP = 0x0020;
		private const uint ERROR_SERVICE_DOES_NOT_EXIST = 1060;
		private const uint SC_STATUS_PROCESS_INFO = 0;
		private const uint SERVICE_RUNNING = 0x00000004;
		private const uint SERVICE_CONTROL_STOP = 0x00000001;
		private const int ERROR_ACCESS_DENIED = 0x5;

		[DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern IntPtr OpenSCManager(string? machineName, string? databaseName, uint dwAccess);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool CloseServiceHandle(IntPtr hSCObject);

		[DllImport("advapi32.dll", EntryPoint = "OpenServiceA", SetLastError = true, CharSet = CharSet.Ansi)]
		private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, uint dwDesiredAccess);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct SERVICE_STATUS
		{
			public int serviceType;
			public int currentState;
			public int controlsAccepted;
			public int win32ExitCode;
			public int serviceSpecificExitCode;
			public int checkPoint;
			public int waitHint;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		private struct SERVICE_STATUS_PROCESS
		{
			public int serviceType;
			public int currentState;
			public int controlsAccepted;
			public int win32ExitCode;
			public int serviceSpecificExitCode;
			public int checkPoint;
			public int waitHint;
			public int processID;
			public int serviceFlags;
		}

		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool QueryServiceStatusEx(IntPtr serviceHandle, uint infoLevel, ref SERVICE_STATUS_PROCESS buffer, int bufferSize, out uint bytesNeeded);

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool ControlService(IntPtr hService, uint dwControl, ref SERVICE_STATUS lpServiceStatus);

		[DllImport("advapi32", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool StartService(IntPtr hService, int dwNumServiceArgs, string[] lpServiceArgVectors);
	}
}
