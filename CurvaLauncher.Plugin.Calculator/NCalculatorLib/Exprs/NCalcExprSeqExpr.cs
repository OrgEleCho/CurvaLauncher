using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    //public abstract class Expr
    //{
    //    public abstract double Eval();
    //}

    public class NCalcExprSeqExpr : NCalcExpr
    {
        public readonly NCalcExpr[] SeqValue;

        public NCalcExprSeqExpr(params NCalcExpr[] exprs)
        {
            if (exprs.Length < 1)
                throw new ArgumentOutOfRangeException();

            SeqValue = exprs;
        }

        public NCalcExpr this[int index] => SeqValue[index];

        public override double Eval(NCalcContext context)
        {
            return SeqValue[0].Eval(context);
        }

        public override Expression ToExpression(NCalcContext context)
        {
            return SeqValue[0].ToExpression(context);
        }

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            for (int i = 0; i < SeqValue.Length - 1; i++)
            {
                foreach (NCalcToken token in SeqValue[i].EnumTokens())
                    yield return token;
                yield return new NCalcToken(NCalcTokenKind.Comma, null);
            }

            foreach (NCalcToken token in SeqValue.Last().EnumTokens())
                yield return token;
        }
    }
}
