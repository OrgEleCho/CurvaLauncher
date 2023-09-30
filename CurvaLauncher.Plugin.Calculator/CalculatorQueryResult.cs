using System.Windows;
using System.Windows.Media;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.Calculator
{
    public class CalculatorQueryResult : QueryResult
    {
        public CalculatorQueryResult(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public override float Weight => 1;

        public override string Title => $"{Value}";

        public override string Description => $"Copy value of calculation result: {Value}";


        public override ImageSource? Icon => null;


        public override void Invoke()
        {
            Clipboard.SetText($"{Value}");
        }
    }
}