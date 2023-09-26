#nullable enable

using System.Linq.Expressions;

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NCalcBinTailExpr : NCalcExpr
    {
        public NCalcBinTailExpr(NCalcToken op, NCalcExpr right, NCalcBinTailExpr? tail)
        {
            Operator = op;
            RightExpr = right;
            TailExpr = tail;
        }

        public NCalcToken Operator { get; }
        public NCalcExpr RightExpr { get; }
        public NCalcBinTailExpr? TailExpr { get; }

        public abstract double Eval(NCalcContext context, double left);

        public override double Eval(NCalcContext context) => throw new InvalidOperationException();

        public abstract Expression ToExpression(NCalcContext context, Expression left);
        public override Expression ToExpression(NCalcContext context) => throw new InvalidOperationException();



        public override IEnumerable<NCalcToken> EnumTokens()
        {
            yield return Operator;
            foreach (var token in RightExpr.EnumTokens())
                yield return token;

            if (TailExpr != null)
            {
                foreach (var token in TailExpr.EnumTokens())
                    yield return token;
            }
        }
    }
}
