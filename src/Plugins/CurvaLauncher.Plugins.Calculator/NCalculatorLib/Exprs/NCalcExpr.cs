#nullable enable

using System.Linq.Expressions;
using NCalculatorLib;


namespace NCalculatorLib.Exprs
{
    public abstract class NCalcExpr
    {
        public abstract double Eval(NCalcContext context);
        public abstract Expression ToExpression(NCalcContext context);

        public Func<double> Compile(NCalcContext context)
        {
            var expr = ToExpression(context);
            return Expression.Lambda<Func<double>>(expr).Compile();
        }

        public double Eval()
        {
            return Eval(NCalcContext.Default);
        }

        public Expression ToExpression()
        {
            return ToExpression(NCalcContext.Default);
        }

        public Func<double> Compile()
        {
            return Compile(NCalcContext.Default);
        }

        public abstract IEnumerable<NCalcToken> EnumTokens();

        public override string ToString()
        {
            return string.Join(' ', EnumTokens());
        }
    }
}
