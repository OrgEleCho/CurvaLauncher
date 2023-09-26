using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;


#nullable enable

namespace NCalculatorLib.Exprs
{
    public class NCalcEqExpr : NCalcExpr
    {
        public NCalcEqExpr(NCalcToken @operator, NCalcExpr leftExpr, NCalcExpr rightExpr)
        {
            Operator = @operator;
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
                NCalcTokenKind.Eq => LeftExpr.Eval(context) == RightExpr.Eval(context) ? 1 : 0,
                NCalcTokenKind.NoEq => LeftExpr.Eval(context) != RightExpr.Eval(context) ? 1 : 0,
                _ => 0
            };
        }

        public override Expression ToExpression(NCalcContext context)
        {
            return Operator.Kind switch
            {
                NCalcTokenKind.Eq => Expression.Condition(Expression.Equal(LeftExpr.ToExpression(context), RightExpr.ToExpression(context)), Expression.Constant(1.0), Expression.Constant(0.0)),
                NCalcTokenKind.NoEq => Expression.Condition(Expression.NotEqual(LeftExpr.ToExpression(context), RightExpr.ToExpression(context)), Expression.Constant(1.0), Expression.Constant(0.0)),
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
