using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugin.Calculator
{
    public class CalculatorQueryResult : ISyncQueryResult
    {
        public CalculatorQueryResult(double value)
        {
            Value = value;
        }

        public double Value { get; }

        public float Weight => 1;

        public string Title => $"{Value}";

        public string Description => $"Copy value of calculation result: {Value}";


        public ImageSource? Icon => null;


        public void Invoke()
        {
            Clipboard.SetText($"{Value}");
        }
    }
}