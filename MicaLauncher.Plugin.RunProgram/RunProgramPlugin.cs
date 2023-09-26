using System.IO;
using System.Windows.Media;
using System.Windows.Threading;
using MicaLauncher.Common;
using MicaLauncher.Data;

namespace MicaLauncher.Plugin.RunProgram
{
    public class RunProgramPlugin : IPlugin
    {
        public ImageSource Icon => null!;

        public IEnumerable<QueryResult> Query(MicaLauncherContext context, string query)
        {
            string? pathVariable = Environment.GetEnvironmentVariable("PATH");
            if (pathVariable == null)
                yield break;

            string[] directories = pathVariable.Split(';');
            foreach (string directory in directories)
            {
                if (!Directory.Exists(directory))
                    continue;

                string filename = Path.Combine(directory, query);
                if (File.Exists(filename))
                {
                    yield return new RunProgramQueryResult(context, filename);
                    yield break;
                }

                filename = Path.Combine(directory, $"{query}.exe");
                if (File.Exists(filename))
                {
                    yield return new RunProgramQueryResult(context, filename);
                    yield break;
                }
            }
        }
    }
}