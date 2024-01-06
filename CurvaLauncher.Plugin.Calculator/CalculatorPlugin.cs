using System.Windows.Media;
using System.Windows.Threading;
using CurvaLauncher.Data;
using CurvaLauncher.Plugin.Calculator.Properties;
using CurvaLauncher.Utilities;
using NCalculatorLib;

namespace CurvaLauncher.Plugin.Calculator
{

    public class CalculatorPlugin : ISyncPlugin
    {
        readonly Lazy<ImageSource> laziedIcon = new Lazy<ImageSource>(() => ImageUtils.CreateFromSvg(Resources.IconSvg)!);

        [PluginOption]
        public string Prefix { get; set; } = "=";

        public ImageSource Icon => laziedIcon.Value;

        public string Name => "Calculator";

        public string Description => "Math calculation";

        public void Init()
        {
            // do nothing
        }

        public IEnumerable<QueryResult> Query(CurvaLauncherContext context, string query)
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