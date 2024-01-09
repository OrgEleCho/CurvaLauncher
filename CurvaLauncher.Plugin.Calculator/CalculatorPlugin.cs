using System.Windows.Media;
using System.Windows.Threading;
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

        public void Initialize() { }
        public void Finish() { }

        public IEnumerable<IQueryResult> Query(CurvaLauncherContext context, string query)
        {
            if (!query.StartsWith(Prefix))
                yield break;

            string expr = query.Substring(Prefix.Length);

            if (NCalc.TryEval(expr, out var result))
                yield return new CalculatorQueryResult(result);
        }
    }
}