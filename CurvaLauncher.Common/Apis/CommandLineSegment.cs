namespace CurvaLauncher.Apis
{
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
