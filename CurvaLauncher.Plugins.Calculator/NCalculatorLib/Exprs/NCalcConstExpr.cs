using System.Linq.Expressions;
using System.Xml.Linq;


#nullable enable

namespace NCalculatorLib.Exprs
{
    public class NCalcConstExpr : NCalcExpr
    {
        public readonly string Name;
        public NCalcConstExpr(string name)
        {
            Name = name;
        }

        public static double GetValue(NCalcContext context, string name)
        {
            double value;
            Func<double>? valueGetter;

            if (context.Constants.TryGetValue(name, out value) ||
                context.DefaultConstants.TryGetValue(name, out value))
                return value;
            else if (context.Variables.TryGetValue(name, out valueGetter) ||
                     context.DefaultVariables.TryGetValue(name, out valueGetter))
                return valueGetter.Invoke();
            else
                return 0.0;
        }

        public override IEnumerable<NCalcToken> EnumTokens()
        {
            yield return new NCalcToken(NCalcTokenKind.Identifier, Name);
        }

        public override double Eval(NCalcContext context)
        {
            return GetValue(context, Name);
        }

        public override Expression ToExpression(NCalcContext context)
        {
            double value;
            Func<double>? valueGetter;

            if (context.Constants.TryGetValue(Name, out value) ||
                context.DefaultConstants.TryGetValue(Name, out value))
                return Expression.Constant(value);
            else if (context.Variables.TryGetValue(Name, out valueGetter) ||
                     context.DefaultVariables.TryGetValue(Name, out valueGetter))
                return Expression.Invoke(Expression.Constant(valueGetter));
            else
                return Expression.Constant(0.0);
        }
    }
}
