using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Calculator
{
    public class CalculatorQueryResult : ISyncActionQueryResult
    {
        private readonly CurvaLauncherContext _hostContext;

        public CalculatorQueryResult(CurvaLauncherContext hostContext, double value)
        {
            _hostContext = hostContext;
            Value = value;
        }

        public double Value { get; }

        public float Weight => 1;

        public string Title => $"{Value}";

        public string Description => $"Copy value of calculation result: {Value}";


        public ImageSource? Icon => null;


        public void Invoke()
        {
            _hostContext.ClipboardApi.SetText($"{Value}");
        }
    }
}