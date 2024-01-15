using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// basic unit
    /// 
    /// | num
    /// | fun_expr
    /// | quote_expr
    /// </summary>
    public class NCalcUnitExpr : NCalcExpr
    {
        public readonly bool Signed;
        public readonly NCalcExpr Value;

        public NCalcUnitExpr(NCalcExpr value, bool signed)
        {
            Value = value;
            Signed = signed;
        }

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            if (Signed)
                yield return new NCalcToken(NCalcTokenKind.Sub, null);

            foreach (NCalcToken token in Value.EnumTokens())
                yield return token;
        }

        public override double Eval(NCalcContext context)
        {
            return Signed ? -Value.Eval(context) : Value.Eval(context);
        }

        public override Expression ToExpression(NCalcContext context)
        {
            return Signed ? Expression.Negate(Value.ToExpression(context)) : Value.ToExpression(context);
        }
    }
}
