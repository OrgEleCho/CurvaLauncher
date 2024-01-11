using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCalculatorLib
{

    public struct NCalcToken
    {
        public NCalcToken(NCalcTokenKind kind, string? value)
        {
            Kind = kind;
            Value = value;
        }

        public readonly NCalcTokenKind Kind;
        public readonly string? Value;

        public override string ToString()
        {
            if (Value != null)
                return Value;

            return Kind switch
            {
                NCalcTokenKind.Identifier => "err",
                NCalcTokenKind.Number => "0",
                NCalcTokenKind.Plus => "+",
                NCalcTokenKind.Sub => "-",
                NCalcTokenKind.Mul => "*",
                NCalcTokenKind.Div => "/",
                NCalcTokenKind.Pow => "^",
                NCalcTokenKind.Mod => "%",
                NCalcTokenKind.LParen => "(",
                NCalcTokenKind.RParen => ")",
                NCalcTokenKind.Gtr => ">",
                NCalcTokenKind.Lss => "<",
                NCalcTokenKind.GtrEq => ">=",
                NCalcTokenKind.LssEq => "<=",
                NCalcTokenKind.NoEq => "!=",
                NCalcTokenKind.Eq => "==",
                NCalcTokenKind.Colon => ":",
                NCalcTokenKind.Question => "?",
                NCalcTokenKind.Comma => ",",

                _ => string.Empty
            };
        }
    }
}
