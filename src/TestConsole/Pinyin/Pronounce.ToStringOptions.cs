namespace TestConsole.Pinyin
{
    public partial record struct Pronounce
    {
        [Flags]
        public enum FormattingOptions
        {
            All = ~0,
            None = 0,
            AllowPinyin = 1,
            AllowTones  = 2,
        }
    }
}
