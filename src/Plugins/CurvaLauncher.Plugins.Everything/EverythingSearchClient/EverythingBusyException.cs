using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EverythingSearchClient
{
	public class EverythingBusyException: Exception
	{
		public EverythingBusyException() : base("Everything service is busy") { }
	}
}
