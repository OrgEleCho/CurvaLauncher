namespace MicaLauncher.Utilities
{
    public static class StringMatchUtils
    {
        public static float FuzzyMatch(string str1, string str2)
        {
            int length1 = str1.Length;
            int length2 = str2.Length;

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
                    int cost = (str1[i-1] == str2[j-1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                      Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                      d[i - 1, j - 1] + cost);
                }
            }

            return 1 - (float)d[length1, length2] / Math.Max(length1, length2);
        }
    }
}