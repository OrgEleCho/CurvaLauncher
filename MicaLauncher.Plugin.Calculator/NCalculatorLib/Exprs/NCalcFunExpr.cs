using System.Linq.Expressions;


#nullable enable

namespace NCalculatorLib.Exprs
{
    /// <summary>
    /// id quote_expr
    /// </summary>
    public class NCalcFunExpr : NCalcExpr
    {
        public readonly NCalcToken Func;
        public readonly NCalcExprSeqExpr Params;

        public NCalcFunExpr(NCalcToken func, NCalcExprSeqExpr param)
        {
            Func = func;
            Params = param;
        }

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            yield return Func;
            yield return new NCalcToken(NCalcTokenKind.LParen, null);

            foreach (NCalcToken token in Params.EnumTokens())
                yield return token;

            yield return new NCalcToken(NCalcTokenKind.RParen, null);
        }

        public override double Eval(NCalcContext context)
        {
            Func<double[], double>? func;
            if (context.Functions.TryGetValue(Func.Value!, out func) ||
                context.DefaultFunctions.TryGetValue(Func.Value!, out func))
                return func.Invoke(Params.SeqValue.Select(expr => expr.Eval(context)).ToArray());

            return 0;
        }

        public override Expression ToExpression(NCalcContext context)
        {
            Func<double[], double>? func;
            if (context.Functions.TryGetValue(Func.Value!, out func) ||
                context.DefaultFunctions.TryGetValue(Func.Value!, out func))
            {
                return Expression.Invoke(Expression.Constant(func), Expression.NewArrayInit(typeof(double), Params.SeqValue.Select(expr => expr.ToExpression(context))));
            }

            return Expression.Constant(0.0);
        }
    }
}
