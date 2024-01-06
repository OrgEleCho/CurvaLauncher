using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Utilities
{
    public static class CommandLineUtils
    {

        /// <summary>
        /// 分割命令行
        /// </summary>
        /// <param name="commandline"></param>
        /// <param name="segments"></param>
        public static void Split(string commandline, out List<CommandLineSegment> segments)
        {
            List<CommandLineSegment> rstBuilder = new();
            StringBuilder temp = new();
            bool quote = false;

            foreach (char i in commandline)
            {
                if (quote)
                {
                    if (i == '"')
                    {
                        rstBuilder.Add(new CommandLineSegment(temp.ToString(), true));
                        temp.Clear();

                        quote = false;
                    }
                    else
                    {
                        temp.Append(i);
                    }
                }
                else
                {
                    if (i == '"')
                    {
                        if (temp.Length > 0)
                        {
                            rstBuilder.Add(new CommandLineSegment(temp.ToString(), false));
                            temp.Clear();
                        }

                        quote = true;
                    }
                    else if (char.IsWhiteSpace(i))
                    {
                        if (temp.Length > 0)
                        {
                            rstBuilder.Add(new CommandLineSegment(temp.ToString(), false));
                            temp.Clear();
                        }
                    }
                    else
                    {
                        temp.Append(i);
                    }
                }
            }

            if (temp.Length > 0)
                rstBuilder.Add(new CommandLineSegment(temp.ToString(), quote));

            segments = rstBuilder;
        }

        public static string Concat(IEnumerable<CommandLineSegment> segments)
        {
            StringBuilder sb = new();

            bool notFirst = false;
            foreach (var segment in segments)
            {
                if (notFirst)
                    sb.Append(' ');

                if (segment.IsQuoted)
                {
                    sb.Append('"');
                    sb.Append(segment.Value);
                    sb.Append('"');
                }
                else
                {
                    sb.Append(segment.Value);
                }

                notFirst = true;
            }

            return sb.ToString();
        }

        public struct CommandLineSegment
        {
            public CommandLineSegment(string value, bool isQuoted)
            {
                Value = value;
                IsQuoted = isQuoted;
            }

            public string Value { get; }
            public bool IsQuoted { get; }
        }
    }
}
