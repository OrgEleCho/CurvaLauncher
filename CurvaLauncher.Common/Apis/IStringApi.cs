namespace CurvaLauncher.Apis;

public interface IStringApi
{
    public IEnumerable<string> SplitToWords(string str);

    public float LevenShtein<T>(IEnumerable<T> value1, IEnumerable<T> value2) where T : IEquatable<T>;
    public float CosineSimilarity<T>(IEnumerable<T> str1, IEnumerable<T> str2) where T : IEquatable<T>;

    public float MatchLevenShtein(string toCheck, string input);
    public float MatchStartLetter(string toCheck, string input);

    public float MatchWords(string toCheck, string input);
    public float MatchLength(string toCheck, string input);

    public float Match(string toCheck, string input);
}