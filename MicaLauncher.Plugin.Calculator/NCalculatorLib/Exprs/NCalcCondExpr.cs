using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    public class NCalcCondExpr : NCalcExpr
    {
        public NCalcCondExpr(NCalcExpr condition, NCalcExpr leftExpr, NCalcExpr rightExpr)
        {
            Condition = condition;
            LeftExpr = leftExpr;
            RightExpr = rightExpr;
        }

        public readonly NCalcExpr Condition;
        public readonly NCalcExpr LeftExpr;
        public readonly NCalcExpr RightExpr;

        public override double Eval(NCalcContext context)
        {
            return Condition.Eval(context) != 0 ? LeftExpr.Eval(context) : RightExpr.Eval(context);
        }

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            foreach (NCalcToken token in Condition.EnumTokens())
                yield return token;

            yield return new NCalcToken(NCalcTokenKind.Question, null);

            foreach (NCalcToken token in LeftExpr.EnumTokens())
                yield return token;

            yield return new NCalcToken(NCalcTokenKind.Colon, null);

            foreach (NCalcToken token in LeftExpr.EnumTokens())
                yield return token;
        }

        public override Expression ToExpression(NCalcContext context)
        {
            return Expression.Condition(Expression.NotEqual(Condition.ToExpression(context), Expression.Constant(0.0)), LeftExpr.ToExpression(context), RightExpr.ToExpression(context));
        }
    }
}
