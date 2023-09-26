using System.Linq.Expressions;

namespace NCalculatorLib.Exprs
{
    public abstract class NCalcBinExpr : NCalcExpr
    {
        public readonly NCalcExpr LeftExpr;
        public readonly NCalcBinTailExpr TailExpr;

        public NCalcBinExpr(NCalcExpr left, NCalcBinTailExpr tail)
        {
            LeftExpr = left;
            TailExpr = tail;
        }

        public override double Eval(NCalcContext context) => TailExpr.Eval(context, LeftExpr.Eval(context));
        public override Expression ToExpression(NCalcContext context) => TailExpr.ToExpression(context, LeftExpr.ToExpression(context));

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            foreach (NCalcToken token in LeftExpr.EnumTokens())
                yield return token;

            foreach (NCalcToken token in TailExpr.EnumTokens())
                yield return token;
        }
    }
}