using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// basic number
    /// 
    /// | num_literal
    /// </summary>
    public class NCalcNumExpr : NCalcExpr
    {
        public readonly string Value;

        public NCalcNumExpr(string value)
        {
            Value = value;
        }

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            yield return new NCalcToken(NCalcTokenKind.Number, Value);
        }

        public override double Eval(NCalcContext context)
        {
            return double.Parse(Value);
        }

        public override Expression ToExpression(NCalcContext context)
        {
            return Expression.Constant(Eval(context));
        }
    }
}
