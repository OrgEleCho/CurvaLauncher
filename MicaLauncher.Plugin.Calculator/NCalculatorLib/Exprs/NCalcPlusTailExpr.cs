using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// plus tail expression :
    /// 
    /// <para>| +|- mul </para>
    /// <para>| +|- unit </para>
    /// <para>| +|- mul plus_tail</para>
    /// <para>| +|- unit plus_tail</para>
    /// </summary>
    public class NCalcPlusTailExpr : NCalcBinTailExpr
    {
        public NCalcPlusTailExpr(NCalcToken op, NCalcExpr right, NCalcPlusTailExpr? tail) : base(op, right, tail)
        {
            if (op.Kind != NCalcTokenKind.Plus && op.Kind != NCalcTokenKind.Sub)
                throw new ArgumentException("Not a Plus/Sub token");
        }

        public override double Eval(NCalcContext context, double left)
        {
            double value = RightExpr.Eval(context);

            if (Operator.Kind == NCalcTokenKind.Sub)
                value = -value;

            value += left;

            if (TailExpr is not null)
                value = TailExpr.Eval(context, value);

            return value;
        }

        public override Expression ToExpression(NCalcContext context, Expression left)
        {
            Expression value = RightExpr.ToExpression(context);

            if (Operator.Kind == NCalcTokenKind.Sub)
                value = Expression.Negate(value);

            value = Expression.Add(left, value);

            if (TailExpr is not null)
                value = TailExpr.ToExpression(context, value);

            return value;
        }
    }
}
