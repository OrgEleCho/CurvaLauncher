using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverythingSearchClient
{
	public class ResultsNotReceivedException : Exception
	{
		public ResultsNotReceivedException() : base("Failed to receive results") { }
	}
}
