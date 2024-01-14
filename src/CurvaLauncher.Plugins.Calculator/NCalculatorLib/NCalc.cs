
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NCalculatorLib.Exprs;
using System.Diagnostics.CodeAnalysis;

namespace NCalculatorLib
{
    public static partial class NCalc
    {
        public static double Eval(string expr)
        {
            return Parse(expr).Eval(NCalcContext.Default);
        }

        public static double Eval(NCalcContext context, string expr)
        {
            return Parse(expr).Eval(context);
        }

        public static bool TryEval(string expr, [NotNullWhen(true)] out double result)
        {
            if (TryParse(expr, out NCalcExpr? nCalcExpr))
            {
                result = nCalcExpr.Eval(NCalcContext.Default);
                return true;
            }
            else
            {
                result = 0;
                return false;
            }
        }

        public static Func<double> Compile(string expr)
        {
            return Parse(expr).Compile(NCalcContext.Default);
        }

        public static Func<double> Compile(NCalcContext context, string expr)
        {
            NCalcToken[] tokens = new NCalcLexer(new StringReader(expr)).Lex().ToArray();
            NCalcParser nCalcParser = new NCalcParser(tokens);
            NCalcExpr _expr = nCalcParser.Parse();
            return Parse(expr).Compile(context);
        }

        public static double CompileEval(string expr)
        {
            return Compile(expr).Invoke();
        }

        public static double CompileEval(NCalcContext context, string expr)
        {
            return Compile(context, expr).Invoke();
        }

        public static NCalcExpr Parse(string expr)
        {
            NCalcToken[] tokens = new NCalcLexer(new StringReader(expr)).Lex().ToArray();
            NCalcParser nCalcParser = new NCalcParser(tokens);
            NCalcExpr _expr = nCalcParser.Parse();
            return _expr;
        }

        public static bool TryParse(string expr, [NotNullWhen(true)] out NCalcExpr? result)
        {
            NCalcToken[] tokens = new NCalcLexer(new StringReader(expr)).Lex().ToArray();
            NCalcParser nCalcParser = new NCalcParser(tokens);

            return nCalcParser.TryParse(out result);
        }
    }
}
