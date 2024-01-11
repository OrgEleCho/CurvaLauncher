using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Apis;

public interface ICommandLineApi
{
    public void Split(string commandLine, out List<CommandLineSegment> segments);
    public string Concat(IEnumerable<CommandLineSegment> segments);
}
