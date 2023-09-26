using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// mul tail expression :
    /// 
    /// <para>| *|/|%|^ unit</para>
    /// <para>| *|/|%|^ unit mul_tail</para>
    /// </summary>
    public class NCalcMulTailExpr : NCalcBinTailExpr
    {
        public NCalcMulTailExpr(NCalcToken op, NCalcExpr right, NCalcBinTailExpr? tail) : base(op, right, tail)
        {
            if (op.Kind != NCalcTokenKind.Mul &&
                op.Kind != NCalcTokenKind.Div &&
                op.Kind != NCalcTokenKind.Mod &&
                op.Kind != NCalcTokenKind.Pow)
                throw new ArgumentException("Not a Mul/Div/Mod/Pow token");
        }

        public override double Eval(NCalcContext context, double left)
        {
            double value = Operator.Kind switch
            {
                NCalcTokenKind.Mul => left * RightExpr.Eval(context),
                NCalcTokenKind.Div => left / RightExpr.Eval(context),
                NCalcTokenKind.Mod => left % RightExpr.Eval(context),
                NCalcTokenKind.Pow => Math.Pow(left, RightExpr.Eval(context)),
                _ => 0
            };

            if (TailExpr is not null)
                value = TailExpr.Eval(context, value);

            return value;
        }

        public override Expression ToExpression(NCalcContext context, Expression left)
        {
            Expression value = Operator.Kind switch
            {
                NCalcTokenKind.Mul => Expression.Multiply(left, RightExpr.ToExpression(context)),
                NCalcTokenKind.Div => Expression.Divide(left, RightExpr.ToExpression(context)),
                NCalcTokenKind.Mod => Expression.Modulo(left, RightExpr.ToExpression(context)),
                NCalcTokenKind.Pow => Expression.Power(left, RightExpr.ToExpression(context)),
                _ => Expression.Constant(0)
            };

            if (TailExpr is not null)
                value = TailExpr.ToExpression(context, value);

            return value;
        }
    }
}
