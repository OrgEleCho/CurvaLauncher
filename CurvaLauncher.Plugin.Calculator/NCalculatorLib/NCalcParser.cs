using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NCalculatorLib.Exprs;


#nullable enable

namespace NCalculatorLib
{
    public class NCalcParser
    {
        public NCalcToken[] Tokens { get; }


        public NCalcParser(NCalcToken[] tokens)
        {
            Tokens = tokens;
        }

        public bool MatchExprSeqExpr(ref int index, [NotNullWhen(true)] out NCalcExprSeqExpr? rst)
        {
            rst = null;
            int tempindex1 = index;
            if (MatchExpr(ref tempindex1, out var firstExpr))
            {
                List<NCalcExpr> exprs = new List<NCalcExpr>() { firstExpr! };

                while (tempindex1 < Tokens.Length)
                {
                    if (Tokens[tempindex1].Kind != NCalcTokenKind.Comma)
                    {
                        index = tempindex1;
                        rst = new NCalcExprSeqExpr(exprs.ToArray());
                        return true;
                    }

                    tempindex1 += 1;
                    if (!MatchExpr(ref tempindex1, out var nextExpr))
                        return false;

                    exprs.Add(nextExpr!);
                }
            }

            return false;
        }

        public bool MatchExpr(ref int index, [NotNullWhen(true)] out NCalcExpr? rst)
        {
            if (MatchLogicExpr(ref index, out var logicExpr))
            {
                rst = logicExpr;
                return true;
            }
            if (MatchMathExpr(ref index, out var mathExpr))
            {
                rst = mathExpr;
                return true;
            }

            rst = null;
            return false;
        }

        public bool MatchMathExpr(ref int index, [NotNullWhen(true)] out NCalcExpr? rst)
        {
            if (MatchPlusExpr(ref index, out var plusExpr))
            {
                rst = plusExpr;
                return true;
            }

            if (MatchMulExpr(ref index, out var mulExpr))
            {
                rst = mulExpr;
                return true;
            }

            if (MatchFunExpr(ref index, out var funExpr))
            {
                rst = funExpr;
                return true;
            }

            if (MatchQuoteExpr(ref index, out var quotedExpr))
            {
                rst = quotedExpr;
                return true;
            }

            if (MatchUnitExpr(ref index, out var unitExpr))
            {
                rst = unitExpr;
                return true;
            }

            rst = null;
            return false;
        }

        public bool MatchLogicExpr(ref int index, [NotNullWhen(true)] out NCalcExpr? rst)
        {
            rst = null;

            int tmpindex = index;
            if (MatchCondExpr(ref tmpindex, out var condExpr))
                rst = condExpr;
            else if (MatchEqExpr(ref tmpindex, out var eqExpr))
                rst = eqExpr;
            else if (MatchRelExpr(ref tmpindex, out var relExpr))
                rst = relExpr;
            else
                return false;

            index = tmpindex;
            return true;
        }

        public bool MatchCondExpr(ref int index, [NotNullWhen(true)] out NCalcCondExpr? rst)
        {
            rst = null;
            int tempindex1 = index;
            if (MatchEqExpr(ref tempindex1, out var eqExpr))
            {
                if (tempindex1 < Tokens.Length && Tokens[tempindex1].Kind != NCalcTokenKind.Question)
                    return false;

                int tempindex2 = tempindex1 + 1;
                if (MatchCondExpr(ref tempindex2, out var condExpr))
                {
                    if (Tokens[tempindex2].Kind != NCalcTokenKind.Colon)
                        return false;

                    int tempindex3 = tempindex2 + 1;
                    if (MatchCondExpr(ref tempindex3, out var condExpr2))
                    {
                        rst = new NCalcCondExpr(eqExpr!, condExpr!, condExpr2!);
                        index = tempindex3;
                        return true;
                    }
                    else if (MatchMathExpr(ref tempindex2, out var mathExpr))
                    {
                        rst = new NCalcCondExpr(eqExpr!, condExpr!, mathExpr!);
                        index = tempindex3;
                        return true;
                    }
                }
                else if (MatchMathExpr(ref tempindex2, out var mathExpr2))
                {
                    if (Tokens[tempindex2].Kind != NCalcTokenKind.Colon)
                        return false;

                    int tempindex3 = tempindex2 + 1;
                    if (MatchMathExpr(ref tempindex3, out var mathExpr3))
                    {
                        rst = new NCalcCondExpr(eqExpr!, mathExpr2!, mathExpr3!);
                        index = tempindex3;
                        return true;
                    }
                }
            }
            else if (MatchRelExpr(ref tempindex1, out var relExpr))
            {
                if (tempindex1 < Tokens.Length && Tokens[tempindex1].Kind != NCalcTokenKind.Question)
                    return false;

                int tempindex2 = tempindex1 + 1;

                if (MatchCondExpr(ref tempindex2, out var condExpr))
                {
                    if (Tokens[tempindex2].Kind != NCalcTokenKind.Colon)
                        return false;

                    int tempindex3 = tempindex2 + 1;
                    if (MatchCondExpr(ref tempindex3, out var condExpr2))
                    {
                        rst = new NCalcCondExpr(relExpr!, condExpr!, condExpr2!);
                        index = tempindex3;
                        return true;
                    }
                    else if (MatchMathExpr(ref tempindex2, out var mathExpr))
                    {
                        rst = new NCalcCondExpr(relExpr!, condExpr!, mathExpr!);
                        index = tempindex3;
                        return true;
                    }
                }
                else if (MatchMathExpr(ref tempindex2, out var mathExpr2))
                {
                    if (Tokens[tempindex2].Kind != NCalcTokenKind.Colon)
                        return false;

                    int tempindex3 = tempindex2 + 1;
                    if (MatchMathExpr(ref tempindex3, out var mathExpr3))
                    {
                        rst = new NCalcCondExpr(relExpr!, mathExpr2!, mathExpr3!);
                        index = tempindex3;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool MatchEqExpr(ref int index, [NotNullWhen(true)] out NCalcEqExpr? rst)
        {
            rst = null;
            int tempindex1 = index;
            if (MatchMathExpr(ref tempindex1, out var mathExpr))
            {
                if (tempindex1 >= Tokens.Length)
                    return false;

                var optToken = Tokens[tempindex1];
                if (optToken.Kind != NCalcTokenKind.Eq &&
                    optToken.Kind != NCalcTokenKind.NoEq)
                    return false;

                int tempindex2 = tempindex1 + 1;
                if (MatchEqExpr(ref tempindex2, out var relExpr2))
                {
                    rst = new NCalcEqExpr(optToken, mathExpr!, relExpr2!);
                    index = tempindex2;
                    return true;
                }
                else if (MatchMathExpr(ref tempindex2, out var mathExpr2))
                {
                    rst = new NCalcEqExpr(optToken, mathExpr!, mathExpr2!);
                    index = tempindex2;
                    return true;
                }
            }

            return false;
        }

        public bool MatchRelExpr(ref int index, [NotNullWhen(true)] out NCalcRelExpr? rst)
        {
            rst = null;
            int tempindex1 = index;
            if (MatchMathExpr(ref tempindex1, out var mathExpr))
            {
                if (tempindex1 >= Tokens.Length)
                    return false;

                var optToken = Tokens[tempindex1];
                if (optToken.Kind != NCalcTokenKind.Gtr &&
                    optToken.Kind != NCalcTokenKind.GtrEq &&
                    optToken.Kind != NCalcTokenKind.Lss &&
                    optToken.Kind != NCalcTokenKind.LssEq)
                    return false;

                int tempindex2 = tempindex1 + 1;
                if (MatchRelExpr(ref tempindex2, out var relExpr2))
                {
                    rst = new NCalcRelExpr(optToken, mathExpr!, relExpr2!);
                    index = tempindex2;
                    return true;
                }
                else if (MatchMathExpr(ref tempindex2, out var mathExpr2))
                {
                    rst = new NCalcRelExpr(optToken, mathExpr!, mathExpr2!);
                    index = tempindex2;
                    return true;
                }
            }

            return false;
        }

        public bool MatchFunExpr(ref int index, [NotNullWhen(true)] out NCalcFunExpr? rst)
        {
            rst = null;
            if (index >= Tokens.Length)
                return false;

            if (Tokens[index].Kind != NCalcTokenKind.Identifier)
                return false;

            int tempindex = index + 1;
            if (MatchQuoteExprSeqExpr(ref tempindex, out var expr))
            {
                rst = new NCalcFunExpr(Tokens[index], expr!);
                index = tempindex;
                return true;
            }

            return false;
        }

        public bool MatchQuoteExprSeqExpr(ref int index, [NotNullWhen(true)] out NCalcExprSeqExpr? rst)
        {
            rst = null;
            if (index >= Tokens.Length)
                return false;

            if (Tokens[index].Kind != NCalcTokenKind.LParen)
                return false;

            int tempindex = index + 1;
            if (MatchExprSeqExpr(ref tempindex, out var expr))
            {
                if (tempindex >= Tokens.Length)
                    return false;

                if (Tokens[tempindex].Kind == NCalcTokenKind.RParen)
                {
                    rst = expr;
                    index = tempindex + 1;
                    return true;
                }
            }

            return false;
        }

        public bool MatchQuoteExpr(ref int index, [NotNullWhen(true)] out NCalcExpr? rst)
        {
            rst = null;
            if (index >= Tokens.Length)
                return false;

            if (Tokens[index].Kind != NCalcTokenKind.LParen)
                return false;

            int tempindex = index + 1;
            if (MatchExpr(ref tempindex, out var expr))
            {
                if (tempindex >= Tokens.Length)
                    return false;

                if (Tokens[tempindex].Kind == NCalcTokenKind.RParen)
                {
                    rst = expr;
                    index = tempindex + 1;
                    return true;
                }
            }

            return false;
        }

        public bool MatchPlusExpr(ref int index, [NotNullWhen(true)] out NCalcPlusExpr? rst)
        {
            rst = null;
            int tmpindex = index;

            NCalcExpr? left;
            if (MatchMulExpr(ref tmpindex, out var mulExpr))
                left = mulExpr;
            else if (MatchUnitExpr(ref tmpindex, out var unitExpr))
                left = unitExpr;
            else
                return false;

            if (MatchPlusTailExpr(ref tmpindex, out NCalcPlusTailExpr? tailExpr))
            {
                index = tmpindex;
                rst = new NCalcPlusExpr(left, tailExpr);
                return true;
            }

            return false;
        }

        public bool MatchPlusTailExpr(ref int index, [NotNullWhen(true)] out NCalcPlusTailExpr? rst)
        {
            rst = null;
            int tmpindex = index;
            if (tmpindex >= Tokens.Length)
                return false;

            NCalcToken optToken = Tokens[tmpindex];
            if (optToken.Kind != NCalcTokenKind.Plus &&
                optToken.Kind != NCalcTokenKind.Sub)
                return false;

            tmpindex++;

            NCalcExpr? rightExpr;
            if (MatchMulExpr(ref tmpindex, out NCalcMulExpr? mulExpr))
                rightExpr = mulExpr;
            else if (MatchUnitExpr(ref tmpindex, out NCalcUnitExpr? unitExpr))
                rightExpr = unitExpr;
            else
                return false;

            MatchPlusTailExpr(ref tmpindex, out NCalcPlusTailExpr? tailExpr);

            index = tmpindex;
            rst = new NCalcPlusTailExpr(optToken, rightExpr, tailExpr);
            return true;
        }

        public bool MatchMulExpr(ref int index, [NotNullWhen(true)] out NCalcMulExpr? rst)
        {
            int tmpindex = index;
            if (MatchUnitExpr(ref tmpindex, out NCalcUnitExpr? unitExpr) &&
                MatchMulTailExpr(ref tmpindex, out NCalcMulTailExpr? tailExpr))
            {
                index = tmpindex;
                rst = new NCalcMulExpr(unitExpr, tailExpr);
                return true;
            }

            rst = null;
            return false;
        }

        public bool MatchMulTailExpr(ref int index, [NotNullWhen(true)] out NCalcMulTailExpr? rst)
        {
            rst = null;
            int tmpindex = index;

            if (tmpindex >= Tokens.Length)
                return false;

            NCalcToken optToken = Tokens[tmpindex];
            if (optToken.Kind != NCalcTokenKind.Mul &&
                optToken.Kind != NCalcTokenKind.Div &&
                optToken.Kind != NCalcTokenKind.Mod &&
                optToken.Kind != NCalcTokenKind.Pow)
                return false;

            tmpindex++;
            if (!MatchUnitExpr(ref tmpindex, out NCalcUnitExpr? unitExpr))
                return false;

            MatchMulTailExpr(ref tmpindex, out NCalcMulTailExpr? tailExpr);

            index = tmpindex;
            rst = new NCalcMulTailExpr(optToken, unitExpr, tailExpr);
            return true;
        }

        public bool MatchUnitExpr(ref int index, [NotNullWhen(true)] out NCalcUnitExpr? rst)
        {
            rst = null;
            bool signed;
            int tempindex = index;

            if (index >= Tokens.Length)
                return false;

            signed = Tokens[index].Kind == NCalcTokenKind.Sub;
            if (signed)
                tempindex++;

            if (MatchNumExpr(ref tempindex, out var numExpr))
            {
                rst = new NCalcUnitExpr(numExpr!, signed);
                index = tempindex;
                return true;
            }
            else if (MatchFunExpr(ref tempindex, out var funExpr))
            {
                rst = new NCalcUnitExpr(funExpr!, signed);
                index = tempindex;
                return true;
            }
            else if (MatchConstExpr(ref tempindex, out var constExpr))
            {
                rst = new NCalcUnitExpr(constExpr!, signed);
                index = tempindex;
                return true;
            }
            else if (MatchQuoteExpr(ref tempindex, out var quoteExpr))
            {
                rst = new NCalcUnitExpr(quoteExpr!, signed);
                index = tempindex;
                return true;
            }

            return false;
        }

        public bool MatchNumExpr(ref int index, [NotNullWhen(true)] out NCalcNumExpr? rst)
        {
            rst = null;
            if (index >= Tokens.Length)
                return false;

            if (Tokens[index].Kind == NCalcTokenKind.Number)
            {
                rst = new NCalcNumExpr(Tokens[index].Value!);
                index += 1;
                return true;
            }

            return false;
        }

        public bool MatchConstExpr(ref int index, [NotNullWhen(true)] out NCalcConstExpr? rst)
        {
            rst = null;
            if (index >= Tokens.Length)
                return false;

            if (Tokens[index].Kind == NCalcTokenKind.Identifier)
            {
                rst = new NCalcConstExpr(Tokens[index].Value!);
                index += 1;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Invalid expression</exception>
        public NCalcExpr Parse()
        {
            int index = 0;
            if (MatchExpr(ref index, out var rst))
            {
                if (index < Tokens.Length)
                    throw new ArgumentException($"Invalid expression, Unexpected token: {Tokens[index]} at index {index}");
                return rst!;
            }
            else
            {
                throw new ArgumentException("Invalid expression");
            }
        }
    }
}
