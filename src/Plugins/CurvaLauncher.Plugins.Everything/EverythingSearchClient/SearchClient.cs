using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;

namespace EverythingSearchClient
{

	[SupportedOSPlatform("windows")]
	public class SearchClient
	{

		private static IpcWindow ipcWindow = new IpcWindow();

		/// <summary>
		/// Predefined search filter string for audio files
		/// </summary>
		public const string FilterAudio = "nofiltercase:nofilterpath:nofilterwholeword:filterdiacritics:nofilterregex:<ext:aac;ac3;aif;aifc;aiff;au;cda;dts;fla;flac;it;m1a;m2a;m3u;m4a;m4b;m4p;mid;midi;mka;mod;mp2;mp3;mpa;ogg;ra;rmi;snd;spc;umx;voc;wav;wma;xm >";

		/// <summary>
		/// Predefined search filter string for compressed archives files
		/// </summary>
		public const string FilterZipped = "nofiltercase:nofilterpath:nofilterwholeword:filterdiacritics:nofilterregex:<ext:7z;ace;arj;bz2;cab;gz;gzip;jar;r00;r01;r02;r03;r04;r05;r06;r07;r08;r09;r10;r11;r12;r13;r14;r15;r16;r17;r18;r19;r20;r21;r22;r23;r24;r25;r26;r27;r28;r29;rar;tar;tgz;z;zip >";

		/// <summary>
		/// Predefined search filter string for documents
		/// </summary>
		public const string FilterDocuments = "nofiltercase:nofilterpath:nofilterwholeword:filterdiacritics:nofilterregex:<ext:c;chm;cpp;csv;cxx;doc;docm;docx;dot;dotm;dotx;h;hpp;htm;html;hxx;ini;java;lua;mht;mhtml;odt;pdf;potx;potm;ppam;ppsm;ppsx;pps;ppt;pptm;pptx;rtf;sldm;sldx;thmx;txt;vsd;wpd;wps;wri;xlam;xls;xlsb;xlsm;xlsx;xltm;xltx;xml >";

		/// <summary>
		/// Predefined search filter string for executable files
		/// </summary>
		public const string FilterExecutables = "nofiltercase:nofilterpath:nofilterwholeword:filterdiacritics:nofilterregex:<ext:bat;cmd;exe;msi;msp;scr >";

		/// <summary>
		/// Predefined search filter string for picture files
		/// </summary>
		public const string FilterPictures = "nofiltercase:nofilterpath:nofilterwholeword:filterdiacritics:nofilterregex:<ext:ani;bmp;gif;ico;jpe;jpeg;jpg;pcx;png;psd;tga;tif;tiff;webp;wmf >";

		/// <summary>
		/// Predefined search filter string for video files
		/// </summary>
		public const string FilterVideo = "nofiltercase:nofilterpath:nofilterwholeword:filterdiacritics:nofilterregex:<ext:3g2;3gp;3gp2;3gpp;amr;amv;asf;avi;bdmv;bik;d2v;divx;drc;dsa;dsm;dss;dsv;evo;f4v;flc;fli;flic;flv;hdmov;ifo;ivf;m1v;m2p;m2t;m2ts;m2v;m4v;mkv;mp2v;mp4;mp4v;mpe;mpeg;mpg;mpls;mpv2;mpv4;mov;mts;ogm;ogv;pss;pva;qt;ram;ratdvd;rm;rmm;rmvb;roq;rpm;smil;smk;swf;tp;tpr;ts;vob;vp6;webm;wm;wmp;wmv >";

		/// <summary>
		/// Checks if the Everything service is available
		/// </summary>
		public static bool IsEverythingAvailable()
        {
            ipcWindow.Detect();
            return ipcWindow.IsAvailable;
		}

		/// <summary>
		/// Gets the version of the Everything service
		/// </summary>
		/// <exception cref="InvalidOperationException">When Everything is not available</exception>
		public static Version GetEverythingVersion()
		{
			if (!IsEverythingAvailable())
			{
				throw new InvalidOperationException("Everything service is not available");
			}
			return ipcWindow.GetVersion();
		}

		/// <summary>
		/// Checks whether the Everything service is currently busy with another query.
		/// </summary>
		/// <exception cref="InvalidOperationException">When Everything is not available</exception>
		public static bool IsEverythingBusy()
		{
			if (!IsEverythingAvailable())
			{
				throw new InvalidOperationException("Everything service is not available");
			}
			return ipcWindow.IsBusy();
		}

		[Flags]
		public enum SearchFlags
		{
			None = 0,
			MatchCase = 1,
			MatchWholeWord = 2, // match whole word
			MatchPath = 4, // include paths in search
			RegEx = 8 // enable regex
		}

		/// <summary>
		/// Sort results by one of these attributes
		/// </summary>
		public enum SortBy
		{
			None,
			Name,
			Path,
			Size,
			Extension,
			DateCreated,
			DateModified
		}

		/// <summary>
		/// Define results sort order direction
		/// </summary>
		public enum SortDirection
		{
			Ascending,
			Decending
		}

		/// <summary>
		/// Defines what the client should do when the Everything service is busy
		/// </summary>
		public enum BehaviorWhenBusy
		{
			/// <summary>
			/// Blockingly waits the specified timeout.
			/// If the service becomes free, the search query will be issued.
			/// Please, note that system-wide race conditions might still be possible.
			/// If the timeout triggers and the service is still blocked, an error is raised.
			/// </summary>
			WaitOrError,

			/// <summary>
			/// Blockingly waits the specified timeout.
			/// If the service becomes free, the search query will be issued.
			/// Please, note that system-wide race conditions might still be possible.
			/// If the timeout triggers and the service is still blocked, the query is issued anyway.
			/// This will abort the currently running query,
			/// which might lead to undefined behavior in other applications
			/// which might be waiting on their results.
			/// </summary>
			WaitOrContinue,

			/// <summary>
			/// Throws an exception if the service is busy.
			/// </summary>
			Error,

			/// <summary>
			/// Continue issuing the query.
			/// This will abort the currently running query,
			/// which might lead to undefined behavior in other applications
			/// which might be waiting on their results.
			/// </summary>
			Continue
		}

		/// <summary>
		/// Setting to restrict which version of the query API of Everything is to be used
		/// </summary>
		public enum QueryApi
		{
			/// <summary>
			/// Tries to call the best API first, and might fallback to other on the loss of functionality
			/// </summary>
			Any,

			/// <summary>
			/// Query API (first version) of Everything IPC
			/// </summary>
			Query1only,

			/// <summary>
			/// Query 2 API of Everything IPC offers access to file times and sizes
			/// This option does not fall back to Query1
			/// </summary>
			Query2only
		}

		/// <summary>
		/// When used for `maxResults`, indicates to return all items
		/// </summary>
		public const uint AllItems = 0xffffffff;

		/// <summary>
		/// The default timeout is 1 minute
		/// </summary>
		public const uint DefaultTimeoutMs = 60 * 1000;

		/// <summary>
		/// Specify which Query API version of Everything ICP to use
		/// </summary>
		public QueryApi UseQueryApi { get; set; } = QueryApi.Any;

		/// <summary>
		/// Specifies the timeout when trying to receive data from Everything IPC
		/// </summary>
		/// <remarks>
		/// The timeout is only measured when Everything is not busy, to exclude the time needed to actually collect the result.
		/// </remarks>
		public TimeSpan ReceiveTimeout { get; set; } = TimeSpan.FromSeconds(5);

		/// <summary>
		/// Issues a search query to the Everything service, waits and returns the Result.
		/// </summary>
		/// <param name="query">The Everything query string</param>
		/// <param name="timeoutMs">Wait timeout in milliseconds. Is only used when `whenBusy` is one of the `Wait*` options.</param>
		/// <exception cref="InvalidOperationException">When Everything is not available</exception>
		public Result Search(
			string query,
			SearchFlags flags = SearchFlags.None,
			uint maxResults = AllItems,
			uint offset = 0,
			BehaviorWhenBusy whenBusy = BehaviorWhenBusy.WaitOrError,
			uint timeoutMs = DefaultTimeoutMs,
			SortBy sortBy = SortBy.None,
			SortDirection sortDirection = SortDirection.Ascending)
		{
			if (!IsEverythingAvailable())
			{
				throw new InvalidOperationException("Everything service is not available");
			}

			QueryApi api = UseQueryApi;

			MessageReceiverWindow myWnd = new();

			bool sent = false;
			// first, try send via ApiV2
			if (api == QueryApi.Any || api == QueryApi.Query2only)
			{
				if (myWnd.BuildQuery2(query, flags, maxResults, offset, sortBy, sortDirection))
				{
					HandleBusyEverything(whenBusy, timeoutMs);
					if (myWnd.SendQuery(ipcWindow.HWnd))
					{
						sent = true;
					}
					else
					{
						// if failing
						if (api == QueryApi.Query2only)
						{
							throw new Exception("Failed to send search query");
						}
						// else, retry with lower api
					}
				}
				else
				{
					if (api == QueryApi.Query2only)
					{
						throw new Exception("Failed to build search query data structure");
					}
					// else, fallback to query1 api when creating query2 failed, and any api version allowed
				}
			}
			if (!sent)
			{
				// second try: Query ApiV1
				if (api == QueryApi.Any || api == QueryApi.Query1only)
				{
					if (!myWnd.BuildQuery(query, flags, maxResults, offset))
					{
						throw new Exception("Failed to build search query data structure");
					}
					HandleBusyEverything(whenBusy, timeoutMs);
					if (myWnd.SendQuery(ipcWindow.HWnd))
					{
						sent = true;
					}
					else
					{
						throw new Exception("Failed to send search query");
					}
				}
			}

			if (sent)
			{
				// Implement timeout
				bool checkForTimeout = true;
				Thread checkTimeout = new Thread(() =>
				{
					DateTime last = DateTime.Now;
					TimeSpan timeout = ReceiveTimeout;
					while (checkForTimeout)
					{
						Thread.Sleep(10);
						if (IsEverythingBusy())
						{
							last = DateTime.Now;
						}
						else if ((DateTime.Now - last) > timeout)
						{
							myWnd.SendClose();
							break;
						}
					}
				});
				checkTimeout.Start();

				myWnd.MessagePump();

				checkForTimeout = false;
				checkTimeout.Join();
			}

			if (myWnd.Result == null)
			{
				throw new ResultsNotReceivedException();
			}

			return myWnd.Result;
		}

		private void HandleBusyEverything(BehaviorWhenBusy whenBusy, uint timeoutMs)
		{
			// Handle busy state of Everything
			if (IsEverythingBusy())
			{
				switch (whenBusy)
				{
					case BehaviorWhenBusy.Continue:
						// just continue
						break;
					case BehaviorWhenBusy.Error:
						throw new EverythingBusyException();
					case BehaviorWhenBusy.WaitOrContinue:
						if (!Wait(timeoutMs))
						{
							goto case BehaviorWhenBusy.Continue;
						}
						break;
					case BehaviorWhenBusy.WaitOrError:
						if (!Wait(timeoutMs))
						{
							goto case BehaviorWhenBusy.Error;
						}
						break;
					default:
						throw new ArgumentException("Unknown whenBusy behavior");
				}
			}
		}

		/// <summary>
		/// Wait for `timeoutMs` milliseconds that the Everything service is no longer busy
		/// </summary>
		/// <param name="timeoutMs">0 means wait indefinitely.</param>
		/// <returns>True if Everything service is ready, False if timeout reached</returns>
		private bool Wait(uint timeoutMs)
		{
			DateTime start = DateTime.Now;
			while (IsEverythingBusy())
			{
				DateTime now = DateTime.Now;
				if (timeoutMs > 0 && (now - start).TotalMilliseconds > timeoutMs)
				{
					return false;
				}
				Thread.Sleep(10);
			}
			return true;
		}

		/// <summary>
		/// Issues a search query to the Everything service, waits and returns the Result.
		/// </summary>
		/// <param name="query">The Everything query string</param>
		/// <param name="timeoutMs">Wait timeout in milliseconds. Is only used when `whenBusy` is one of the `Wait*` options.</param>
		/// <exception cref="InvalidOperationException">When Everything is not available</exception>
		public Result Search(string query, SearchFlags flags, BehaviorWhenBusy whenBusy, uint timeoutMs = DefaultTimeoutMs)
		{
			return Search(query, flags, AllItems, 0, whenBusy, timeoutMs);
		}

		/// <summary>
		/// Issues a search query to the Everything service, waits and returns the Result.
		/// </summary>
		/// <param name="query">The Everything query string</param>
		/// <param name="timeoutMs">Wait timeout in milliseconds. Is only used when `whenBusy` is one of the `Wait*` options.</param>
		/// <exception cref="InvalidOperationException">When Everything is not available</exception>
		public Result Search(string query, BehaviorWhenBusy whenBusy, uint timeoutMs = DefaultTimeoutMs)
		{
			return Search(query, SearchFlags.None, AllItems, 0, whenBusy, timeoutMs);
		}

		/// <summary>
		/// Issues a search query to the Everything service, waits and returns the Result.
		/// </summary>
		/// <param name="query">The Everything query string</param>
		/// <param name="timeoutMs">Wait timeout in milliseconds. Is only used when `whenBusy` is one of the `Wait*` options.</param>
		/// <exception cref="InvalidOperationException">When Everything is not available</exception>
		public Result Search(string query, uint maxResults, uint offset = 0, BehaviorWhenBusy whenBusy = BehaviorWhenBusy.WaitOrError, uint timeoutMs = DefaultTimeoutMs)
		{
			return Search(query, SearchFlags.None, maxResults, offset, whenBusy, timeoutMs);
		}

		/// <summary>
		/// Issues a search query to the Everything service, waits and returns the Result.
		/// </summary>
		/// <param name="query">The Everything query string</param>
		/// <param name="timeoutMs">Wait timeout in milliseconds. Is only used when `whenBusy` is one of the `Wait*` options.</param>
		/// <exception cref="InvalidOperationException">When Everything is not available</exception>
		public Result Search(string query, uint maxResults, BehaviorWhenBusy whenBusy, uint timeoutMs = DefaultTimeoutMs)
		{
			return Search(query, SearchFlags.None, maxResults, 0, whenBusy, timeoutMs);
		}

	}
}
