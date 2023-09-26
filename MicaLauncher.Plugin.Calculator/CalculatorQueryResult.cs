using System.Windows;
using System.Windows.Media;
using MicaLauncher.Data;

namespace MicaLauncher.Plugin.Calculator
{
    public class CalculatorQueryResult : QueryResult
    {
        public CalculatorQueryResult(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override float Match => 1;

        public override string Title => $"Value: {Value}";

        public override string Description => $"From Calculator Result";


        public override ImageSource? Icon => null;


        public override void Invoke()
        {
            Clipboard.SetText($"{Value}");
        }
    }
}