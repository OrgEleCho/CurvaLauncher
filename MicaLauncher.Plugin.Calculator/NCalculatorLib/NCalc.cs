
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using NCalculatorLib.Exprs;

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
    }
}
