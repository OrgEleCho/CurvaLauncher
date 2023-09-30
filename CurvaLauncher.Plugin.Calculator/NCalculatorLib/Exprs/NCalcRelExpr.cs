using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    public class NCalcRelExpr : NCalcExpr
    {
        public NCalcRelExpr(NCalcToken opt, NCalcExpr leftExpr, NCalcExpr rightExpr)
        {
            Operator = opt;
            LeftExpr = leftExpr;
            RightExpr = rightExpr;
        }

        public readonly NCalcToken Operator;
        public readonly NCalcExpr LeftExpr;
        public readonly NCalcExpr RightExpr;

        public override double Eval(NCalcContext context)
        {
            return Operator.Kind switch
            {
                NCalcTokenKind.Gtr => LeftExpr.Eval(context) > RightExpr.Eval(context) ? 1 : 0,
                NCalcTokenKind.Lss => LeftExpr.Eval(context) < RightExpr.Eval(context) ? 1 : 0,
                NCalcTokenKind.GtrEq => LeftExpr.Eval(context) >= RightExpr.Eval(context) ? 1 : 0,
                NCalcTokenKind.LssEq => LeftExpr.Eval(context) <= RightExpr.Eval(context) ? 1 : 0,
                _ => 0
            };
        }

        public override Expression ToExpression(NCalcContext context)
        {
            return Operator.Kind switch
            {
                NCalcTokenKind.Gtr => Expression.Condition(Expression.GreaterThan(LeftExpr.ToExpression(context), RightExpr.ToExpression(context)), Expression.Constant(1.0), Expression.Constant(0.0)),
                NCalcTokenKind.Lss => Expression.Condition(Expression.LessThan(LeftExpr.ToExpression(context), RightExpr.ToExpression(context)), Expression.Constant(1.0), Expression.Constant(0.0)),
                NCalcTokenKind.GtrEq => Expression.Condition(Expression.GreaterThanOrEqual(LeftExpr.ToExpression(context), RightExpr.ToExpression(context)), Expression.Constant(1.0), Expression.Constant(0.0)),
                NCalcTokenKind.LssEq => Expression.Condition(Expression.LessThanOrEqual(LeftExpr.ToExpression(context), RightExpr.ToExpression(context)), Expression.Constant(1.0), Expression.Constant(0.0)),
                _ => Expression.Constant(0.0)
            };
        }

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            foreach (NCalcToken token in LeftExpr.EnumTokens())
                yield return token;

            yield return Operator;

            foreach (NCalcToken token in RightExpr.EnumTokens())
                yield return token;
        }
    }
}
