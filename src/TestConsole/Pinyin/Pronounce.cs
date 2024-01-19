using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole.Pinyin
{
    public partial record struct Pronounce
    {
        /// <summary>
        /// 声母
        /// </summary>
        public Consonant? Consonant { get; set; }

        /// <summary>
        /// 韵母
        /// </summary>
        public Vowel Vowel { get; set; }

        /// <summary>
        /// 介母 (三音节字会有介母)
        /// </summary>
        public Vowel? SemiVowel { get; set; }

        /// <summary>
        /// 声调
        /// </summary>
        public Tone? Tone { get; set; }



        public Pronounce(Consonant start, Vowel end)
        {
            Consonant = start;
            Vowel = end;
        }

        public Pronounce(Consonant start, Vowel middle, Vowel end)
        {
            Consonant = start;
            SemiVowel = middle;
            Vowel = end;
        }

        public static bool TryParse(string? str, out Pronounce pronounce)
        {
            pronounce = new();
            if (string.IsNullOrWhiteSpace(str))
                return false;

            int cursor = 0;
            Consonant? consonant = char.ToLower(str[0]) switch
            {
                'b' => Pinyin.Consonant.B,
                'p' => Pinyin.Consonant.P,
                'm' => Pinyin.Consonant.M,
                'f' => Pinyin.Consonant.F,
                'd' => Pinyin.Consonant.D,
                't' => Pinyin.Consonant.T,
                'n' => Pinyin.Consonant.N,
                'l' => Pinyin.Consonant.L,
                'g' => Pinyin.Consonant.G,
                'k' => Pinyin.Consonant.K,
                'h' => Pinyin.Consonant.H,
                'j' => Pinyin.Consonant.J,
                'q' => Pinyin.Consonant.Q,
                'x' => Pinyin.Consonant.X,
                'r' => Pinyin.Consonant.R,
                'z' => Pinyin.Consonant.Z,
                'c' => Pinyin.Consonant.C,
                's' => Pinyin.Consonant.S,
                'y' => Pinyin.Consonant.Y,
                'w' => Pinyin.Consonant.W,

                _ => null,
            };

            if (consonant != null)
            {
                cursor++;
                // 没有下一个字母

                if (cursor >= str.Length)
                    return false;

                if (str[cursor] is 'h')
                {
                    consonant = consonant switch
                    {
                        Pinyin.Consonant.Z => Pinyin.Consonant.Zh,
                        Pinyin.Consonant.C => Pinyin.Consonant.Ch,
                        Pinyin.Consonant.S => Pinyin.Consonant.Sh,

                        _ => null
                    };

                    // 现有声母无法与 h 组成新的声母
                    if (consonant == null)
                        return false;

                    cursor++;
                }
            }

            Vowel? semiVowel = null;
            Vowel? finalVowel = null;
            Tone? tone = null;

            for (; cursor < str.Length; cursor++)
            {
                char current = char.ToLower(str[cursor]);

                switch (current)
                {
                    case 'ā':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.One;
                        goto case 'a';
                    case 'á':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Two;
                        goto case 'a';
                    case 'ǎ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Three;
                        goto case 'a';
                    case 'à':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Four;
                        goto case 'a';

                    case 'ō':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.One;
                        goto case 'o';
                    case 'ó':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Two;
                        goto case 'o';
                    case 'ǒ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Three;
                        goto case 'o';
                    case 'ò':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Four;
                        goto case 'o';

                    case 'ē':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.One;
                        goto case 'e';
                    case 'é':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Two;
                        goto case 'e';
                    case 'ě':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Three;
                        goto case 'e';
                    case 'è':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Four;
                        goto case 'e';

                    case 'ī':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.One;
                        goto case 'i';
                    case 'í':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Two;
                        goto case 'i';
                    case 'ǐ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Three;
                        goto case 'i';
                    case 'ì':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Four;
                        goto case 'i';

                    case 'ū':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.One;
                        goto case 'u';
                    case 'ú':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Two;
                        goto case 'u';
                    case 'ǔ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Three;
                        goto case 'u';
                    case 'ù':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Four;
                        goto case 'u';

                    case 'ü':
                        goto case 'v';
                    case 'ǖ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.One;
                        goto case 'v';
                    case 'ǘ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Two;
                        goto case 'v';
                    case 'ǚ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Three;
                        goto case 'v';
                    case 'ǜ':
                        if (tone != null)
                            return false;
                        tone = Pinyin.Tone.Four;
                        goto case 'v';

                    case 'a':
                    {
                        if (finalVowel == null)
                        {
                            finalVowel = Vowel.A;
                            continue;
                        }

                        // 已经有介母了, 不能创建新的介母
                        if (semiVowel != null)
                            return false;

                        // 介母只能是 i 或 u
                        if (finalVowel is not Vowel.I and not Vowel.U)
                            return false;

                        semiVowel = finalVowel;
                        finalVowel = Vowel.A;

                        break;
                    }

                    case 'o':
                    {
                        if (finalVowel == null)
                        {
                            finalVowel = Vowel.O;
                            continue;
                        }

                        if (finalVowel is Vowel.A)
                        {
                            finalVowel = Vowel.Ao;
                            continue;
                        }

                        // 已经有介母了, 不能创建新的介母
                        if (semiVowel != null)
                            return false;

                        // 介母只能是 i 或 u
                        if (finalVowel is not Vowel.I and not Vowel.U)
                            return false;

                        semiVowel = finalVowel;
                        finalVowel = Vowel.O;

                        break;
                    }

                    case 'e':
                    {
                        if (finalVowel == null)
                        {
                            finalVowel = Vowel.E;
                            continue;
                        }

                        Vowel? newFinalVowel = finalVowel switch
                        {
                            Vowel.I => Vowel.Ie,
                            Vowel.U or Vowel.V => Vowel.Ve,
                            _ => null,
                        };

                        if (!newFinalVowel.HasValue)
                            return false;

                        finalVowel = newFinalVowel.Value;
                        break;
                    }

                    case 'i':
                    {
                        if (finalVowel == null)
                        {
                            finalVowel = Vowel.I;
                            continue;
                        }

                        Vowel? newFinalVowel = finalVowel switch
                        {
                            Vowel.A => Vowel.Ai,
                            Vowel.E => Vowel.Ei,
                            Vowel.U => Vowel.Ui,
                            _ => null
                        };

                        if (!newFinalVowel.HasValue)
                            return false;

                        finalVowel = newFinalVowel.Value;
                        break;
                    }

                    case 'u':
                    {
                        if (finalVowel == null)
                        {
                            finalVowel = Vowel.U;
                            continue;
                        }

                        Vowel? newFinalVowel = finalVowel switch
                        {
                            Vowel.O => Vowel.Ou,
                            Vowel.I => Vowel.Iu,
                            _ => null
                        };

                        if (!newFinalVowel.HasValue)
                            return false;

                        finalVowel = newFinalVowel.Value;
                        break;
                    }

                    case 'v':
                    {
                        if (finalVowel == null)
                        {
                            finalVowel = Vowel.V;
                            continue;
                        }

                        // 无法合并
                        return false;
                    }

                    case 'r':
                    {
                        if (finalVowel != Vowel.E)
                            return false;

                        finalVowel = Vowel.Er;
                        break;
                    }

                    case 'n':
                    {
                        Vowel? newFinalVowel = finalVowel switch
                        {
                            Vowel.A => Vowel.An,
                            Vowel.E => Vowel.En,
                            Vowel.I => Vowel.In,
                            Vowel.U => Vowel.Un,
                            Vowel.V => Vowel.Vn,
                            _ => null
                        };

                        if (!newFinalVowel.HasValue)
                        {
                            if (finalVowel != Vowel.O)
                                return false;

                            int cursorNext = cursor + 1;
                            if (cursorNext >= str.Length)
                                return false;

                            if (char.ToLower(str[cursorNext]) != 'g')
                                return false;

                            newFinalVowel = Vowel.Ong;
                            cursor++;
                        }

                        finalVowel = newFinalVowel.Value;
                        break;
                    }

                    case 'g':
                    {
                        Vowel? newFinalVowel = finalVowel switch
                        {
                            Vowel.An => Vowel.Ang,
                            Vowel.En => Vowel.Eng,
                            Vowel.In => Vowel.Ing,
                            _ => null
                        };

                        if (!newFinalVowel.HasValue)
                            return false;

                        finalVowel = newFinalVowel.Value;
                        break;
                    }
                }
            }

            if (!finalVowel.HasValue)
                return false;

            //if (!consonant.HasValue)
            //{
            //    if (finalVowel 
            //        is not Vowel.A 
            //        and not Vowel.Ai 
            //        and not Vowel.E 
            //        and not Vowel.Ei 
            //        and not Vowel.Er)
            //        return false;
            //}

            pronounce.Consonant = consonant;
            pronounce.SemiVowel = semiVowel;
            pronounce.Vowel = finalVowel.Value;
            return true;
        }

        public static Pronounce Parse(string? str)
        {
            if (!TryParse(str, out var pronounce))
                throw new FormatException();

            return pronounce;
        }

        public string ToString(FormattingOptions options)
        {
            StringBuilder sb = new();

            if (Consonant is Pinyin.Consonant consonant)
                sb.Append(consonant.ToString().ToLower());

            if (SemiVowel is Vowel semiVowel)
                sb.Append(semiVowel.ToString().ToLower());

            if (options.HasFlag(FormattingOptions.AllowPinyin))
            {
                if (options.HasFlag(FormattingOptions.AllowTones) && Tone is Pinyin.Tone tone)
                {
                    sb.Append(Vowel switch
                    {
                        Vowel.A => tone switch
                        {
                            Pinyin.Tone.One => "ā",
                            Pinyin.Tone.Two => "á",
                            Pinyin.Tone.Three => "ǎ",
                            Pinyin.Tone.Four => "à",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.O => tone switch
                        {
                            Pinyin.Tone.One => "ō",
                            Pinyin.Tone.Two => "ó",
                            Pinyin.Tone.Three => "ǒ",
                            Pinyin.Tone.Four => "ò",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.E => tone switch
                        {
                            Pinyin.Tone.One => "ē",
                            Pinyin.Tone.Two => "é",
                            Pinyin.Tone.Three => "ě",
                            Pinyin.Tone.Four => "è",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.I => tone switch
                        {
                            Pinyin.Tone.One => "ī",
                            Pinyin.Tone.Two => "í",
                            Pinyin.Tone.Three => "ǐ",
                            Pinyin.Tone.Four => "ì",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.U => tone switch
                        {
                            Pinyin.Tone.One => "ū",
                            Pinyin.Tone.Two => "ú",
                            Pinyin.Tone.Three => "ǔ",
                            Pinyin.Tone.Four => "ù",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.V => tone switch
                        {
                            Pinyin.Tone.One => "ǖ",
                            Pinyin.Tone.Two => "ǘ",
                            Pinyin.Tone.Three => "ǚ",
                            Pinyin.Tone.Four => "ǜ",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ai => tone switch
                        {
                            Pinyin.Tone.One => "aī",
                            Pinyin.Tone.Two => "aí",
                            Pinyin.Tone.Three => "aǐ",
                            Pinyin.Tone.Four => "aì",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ei => tone switch
                        {
                            Pinyin.Tone.One => "eī",
                            Pinyin.Tone.Two => "eí",
                            Pinyin.Tone.Three => "eǐ",
                            Pinyin.Tone.Four => "eì",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ui => tone switch
                        {
                            Pinyin.Tone.One => "ūi",
                            Pinyin.Tone.Two => "úi",
                            Pinyin.Tone.Three => "ǔi",
                            Pinyin.Tone.Four => "ùi",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ao => tone switch
                        {
                            Pinyin.Tone.One => "aō",
                            Pinyin.Tone.Two => "aó",
                            Pinyin.Tone.Three => "aǒ",
                            Pinyin.Tone.Four => "aò",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ou => tone switch
                        {
                            Pinyin.Tone.One => "oū",
                            Pinyin.Tone.Two => "oú",
                            Pinyin.Tone.Three => "oǔ",
                            Pinyin.Tone.Four => "où",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Iu => tone switch
                        {
                            Pinyin.Tone.One => "iū",
                            Pinyin.Tone.Two => "iú",
                            Pinyin.Tone.Three => "iǔ",
                            Pinyin.Tone.Four => "iù",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ie => tone switch
                        {
                            Pinyin.Tone.One => "īe",
                            Pinyin.Tone.Two => "íe",
                            Pinyin.Tone.Three => "ǐe",
                            Pinyin.Tone.Four => "ìe",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ve => tone switch
                        {
                            Pinyin.Tone.One => "vē",
                            Pinyin.Tone.Two => "vé",
                            Pinyin.Tone.Three => "vě",
                            Pinyin.Tone.Four => "vè",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Er => tone switch
                        {
                            Pinyin.Tone.One => "ēr",
                            Pinyin.Tone.Two => "ér",
                            Pinyin.Tone.Three => "ěr",
                            Pinyin.Tone.Four => "èr",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.An => tone switch
                        {
                            Pinyin.Tone.One => "ān",
                            Pinyin.Tone.Two => "án",
                            Pinyin.Tone.Three => "ǎn",
                            Pinyin.Tone.Four => "àn",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.En => tone switch
                        {
                            Pinyin.Tone.One => "ēn",
                            Pinyin.Tone.Two => "én",
                            Pinyin.Tone.Three => "ěn",
                            Pinyin.Tone.Four => "èn",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.In => tone switch
                        {
                            Pinyin.Tone.One => "īn",
                            Pinyin.Tone.Two => "ín",
                            Pinyin.Tone.Three => "ǐn",
                            Pinyin.Tone.Four => "ìn",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Un => tone switch
                        {
                            Pinyin.Tone.One => "ūn",
                            Pinyin.Tone.Two => "ún",
                            Pinyin.Tone.Three => "ǔn",
                            Pinyin.Tone.Four => "ùn",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Vn => tone switch
                        {
                            Pinyin.Tone.One => "ǖn",
                            Pinyin.Tone.Two => "ǘn",
                            Pinyin.Tone.Three => "ǚn",
                            Pinyin.Tone.Four => "ǜn",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ang => tone switch
                        {
                            Pinyin.Tone.One => "āng",
                            Pinyin.Tone.Two => "áng",
                            Pinyin.Tone.Three => "ǎng",
                            Pinyin.Tone.Four => "àng",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Eng => tone switch
                        {
                            Pinyin.Tone.One => "ēng",
                            Pinyin.Tone.Two => "éng",
                            Pinyin.Tone.Three => "ěng",
                            Pinyin.Tone.Four => "èng",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ing => tone switch
                        {
                            Pinyin.Tone.One => "īng",
                            Pinyin.Tone.Two => "íng",
                            Pinyin.Tone.Three => "ǐng",
                            Pinyin.Tone.Four => "ìng",
                            _ => throw new Exception("This would never happened")
                        },

                        Vowel.Ong => tone switch
                        {
                            Pinyin.Tone.One => "ōng",
                            Pinyin.Tone.Two => "óng",
                            Pinyin.Tone.Three => "ǒng",
                            Pinyin.Tone.Four => "òng",
                            _ => throw new Exception("This would never happened")
                        },

                        _ => throw new Exception("This would never happened")
                    });
                }
                else
                {
                    sb.Append(Vowel.ToString().ToLower());
                }

                if (Consonant is not Pinyin.Consonant.J and not Pinyin.Consonant.Q and not Pinyin.Consonant.X and not Pinyin.Consonant.Y)
                    sb.Replace('v', 'ü');
                else
                    sb.Replace('v', 'u');
            }
            else
            {
                sb.Append(Vowel.ToString().ToLower());
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return this.ToString(FormattingOptions.All);
        }
    }
}
