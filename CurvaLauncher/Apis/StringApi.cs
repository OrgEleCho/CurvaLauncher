using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CurvaLauncher.Apis;

public class StringApi : IStringApi
{
    private StringApi() { }

    public static StringApi Instance { get; } = new StringApi();


    public IEnumerable<string> SplitToWords(string str)
    {
        string pattern = @"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])|[-_]| ";

        return Regex.Split(str, pattern).Where(x => !string.IsNullOrWhiteSpace(x));
    }

    public float LevenShtein<T>(IEnumerable<T> value1, IEnumerable<T> value2)
        where T : IEquatable<T>
    {
        List<T> _value1 = value1.ToList();
        List<T> _value2 = value2.ToList();

        int length1 = _value1.Count;
        int length2 = _value2.Count;

        int[,] d = new int[length1 + 1, length2 + 1];

        for (int i = 0; i <= length1; i++)
        {
            d[i, 0] = i;
        }

        for (int j = 0; j <= length2; j++)
        {
            d[0, j] = j;
        }

        for (int i = 1; i <= length1; i++)
        {
            for (int j = 1; j <= length2; j++)
            {
                int cost = (_value1[i - 1].Equals(_value2[j - 1])) ? 0 : 1;

                d[i, j] = Math.Min(
                  Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                  d[i - 1, j - 1] + cost);
            }
        }

        return 1 - (float)d[length1, length2] / Math.Max(length1, length2);
    }

    public float CosineSimilarity<T>(IEnumerable<T> value1, IEnumerable<T> value2) where T : IEquatable<T>
    {
        //去重
        T[] sl = value1.Union(value2).ToArray();

        //获取重复次数
        List<int> arrA = new();
        List<int> arrB = new();
        foreach (var c in sl)
        {
            arrA.Add(value1.Where(x => x.Equals(c)).Count());
            arrB.Add(value2.Where(x => x.Equals(c)).Count());
        }
        //计算商
        float num = 0;
        //被除数
        float numA = 0;
        float numB = 0;
        for (int i = 0; i < sl.Length; i++)
        {
            num += arrA[i] * arrB[i];
            numA += MathF.Pow(arrA[i], 2);
            numB += MathF.Pow(arrB[i], 2);
        }
        float cos = num / (MathF.Sqrt(numA) * MathF.Sqrt(numB));
        return cos;
    }


    public float MatchLevenShtein(string tocheck, string input)
    {
        return LevenShtein(tocheck, input);
    }

    public float MatchStartLetter(string tocheck, string input)
    {
        string wordStartLetters = new string(SplitToWords(tocheck).Select(word => word[0]).ToArray());

        return MatchLevenShtein(wordStartLetters, input);
    }

    public float MatchWords(string tocheck, string input)
    {
        return LevenShtein(SplitToWords(tocheck), SplitToWords(input));
    }

    public float MatchLength(string tocheck, string input)
    {
        return MathF.Min(tocheck.Length, input.Length) / MathF.Max(tocheck.Length, input.Length);
    }

    public float Match(string tocheck, string input)
    {
        float weight1 = MatchLevenShtein(tocheck, input) * 0.2f;
        float weight2 = MatchStartLetter(tocheck, input) * 0.2f;
        float weight3 = MatchWords(tocheck, input) * 0.4f;
        float weight4 = (float)CosineSimilarity(tocheck, input) * 0.2f;
        //float weight4 = MatchLength(tocheck, input);

        return weight1 + weight2 + weight3 + weight4;
        //return (weight1 + weight2 + weight3 + weight4) / 4;
    }
}