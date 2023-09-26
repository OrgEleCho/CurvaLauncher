using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCalculatorLib
{
    public class NCalcLexer
    {
        private readonly TextReader baseReader;

        public TextReader BaseReader => baseReader;
        public NCalcLexer(TextReader tr)
        {
            baseReader = tr ?? throw new ArgumentNullException(nameof(tr));
        }

        public IEnumerable<NCalcToken> Lex()
        {
            while (true)
            {
                var ch = baseReader.Read();
                char cch = (char)ch;

                if (ch == -1)
                    yield break;

                if (char.IsWhiteSpace(cch))
                    continue;
                else if (char.IsLetter(cch))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(cch);

                    while (true)
                    {
                        ch = baseReader.Peek();
                        cch = (char)ch;

                        if (char.IsLetterOrDigit(cch))
                        {
                            baseReader.Read();
                            sb.Append(cch);
                        }
                        else
                        {
                            break;
                        }
                    }

                    yield return new NCalcToken(NCalcTokenKind.Identifier, sb.ToString());
                    continue;
                }
                else if (char.IsDigit(cch))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(cch);

                    while (ch != -1)
                    {
                        ch = baseReader.Peek();
                        cch = (char)ch;
                        if (ch >= '0' && ch <= '9')
                        {
                            baseReader.Read();
                            sb.Append(cch);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (cch == '.')
                    {
                        ch = baseReader.Read();  // skip '.'
                        sb.Append(cch);
                        while (ch != -1)
                        {
                            ch = baseReader.Peek();
                            cch = (char)ch;
                            if (ch >= '0' && ch <= '9')
                            {
                                baseReader.Read();
                                sb.Append(cch);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (cch == 'e' || cch == 'E')
                    {
                        baseReader.Read();  // skip 'e'
                        sb.Append(cch);

                        ch = baseReader.Read();
                        if (ch >= '0' || ch <= '9' || ch == '+' || ch == '-')
                        {
                            baseReader.Read();
                            sb.Append((char)ch);
                        }

                        while (ch != -1)
                        {
                            ch = baseReader.Peek();
                            cch = (char)ch;
                            if (ch >= '0' && ch <= '9')
                            {
                                baseReader.Read();
                                sb.Append(cch);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    yield return new NCalcToken(NCalcTokenKind.Number, sb.ToString());
                    continue;
                }
                else
                {
                    switch (ch)
                    {
                        case '+':
                            yield return new NCalcToken(NCalcTokenKind.Plus, null);
                            break;
                        case '-':
                            yield return new NCalcToken(NCalcTokenKind.Sub, null);
                            break;
                        case '*':
                            if (baseReader.Peek() == '*')
                            {
                                baseReader.Read();
                                yield return new NCalcToken(NCalcTokenKind.Pow, null);
                            }
                            else
                            {
                                yield return new NCalcToken(NCalcTokenKind.Mul, null);
                            }
                            break;
                        case '/':
                            yield return new NCalcToken(NCalcTokenKind.Div, null);
                            break;
                        case '%':
                            yield return new NCalcToken(NCalcTokenKind.Mod, null);
                            break;
                        case '(':
                            yield return new NCalcToken(NCalcTokenKind.LParen, null);
                            break;
                        case ')':
                            yield return new NCalcToken(NCalcTokenKind.RParen, null);
                            break;
                        case '?':
                            yield return new NCalcToken(NCalcTokenKind.Question, null);
                            break;
                        case ':':
                            yield return new NCalcToken(NCalcTokenKind.Colon, null);
                            break;
                        case ',':
                            yield return new NCalcToken(NCalcTokenKind.Comma, null);
                            break;
                        case '^':
                            yield return new NCalcToken(NCalcTokenKind.Pow, null);
                            break;
                        case '>':
                            if (baseReader.Peek() == '=')
                            {
                                baseReader.Read();
                                yield return new NCalcToken(NCalcTokenKind.GtrEq, null);
                            }
                            else if (baseReader.Peek() == '<')
                            {
                                baseReader.Read();
                                yield return new NCalcToken(NCalcTokenKind.NoEq, null);
                            }
                            else
                            {
                                yield return new NCalcToken(NCalcTokenKind.Gtr, null);
                            }
                            break;
                        case '<':
                            if (baseReader.Peek() == '=')
                            {
                                baseReader.Read();
                                yield return new NCalcToken(NCalcTokenKind.LssEq, null);
                            }
                            else if (baseReader.Peek() == '>')
                            {
                                baseReader.Read();
                                yield return new NCalcToken(NCalcTokenKind.NoEq, null);
                            }
                            else
                            {
                                yield return new NCalcToken(NCalcTokenKind.Lss, null);
                            }
                            break;
                        case '=':
                            if (baseReader.Peek() == '=')
                            {
                                baseReader.Read();
                                yield return new NCalcToken(NCalcTokenKind.Eq, null);
                            }
                            else
                            {
                                yield return new NCalcToken(NCalcTokenKind.Eq, null);
                            }
                            break;
                    }
                }
            }
        }
    }
}