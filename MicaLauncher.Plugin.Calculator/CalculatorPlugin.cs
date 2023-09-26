using System.Windows.Media;
using System.Windows.Threading;
using MicaLauncher.Common;
using MicaLauncher.Data;
using NCalculatorLib;

namespace MicaLauncher.Plugin.Calculator
{

    public class CalculatorPlugin : IPlugin
    {
        [PluginOption]
        public string Prefix { get; set; } = "=";

        public ImageSource Icon => null!;

        public IEnumerable<QueryResult> Query(MicaLauncherContext context, string query)
        {
            if (!query.StartsWith(Prefix))
                yield break;

            string expr = query.Substring(Prefix.Length);
            double result;

            try
            {
                result = NCalc.Eval(expr);
            }
            catch
            {
                yield break;
            }

            yield return new CalculatorQueryResult(result);
        }
    }
}