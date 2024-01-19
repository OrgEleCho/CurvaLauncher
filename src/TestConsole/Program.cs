using CurvaLauncher.Apis;
using CurvaLauncher.Utilities;


while (true)
{
    Console.Write(">>> ");
    string? pinyin = Console.ReadLine();

    if (TestConsole.Pinyin.Pronounce.TryParse(pinyin, out var pronounce))
    {
        Console.WriteLine($"结果: 声母{pronounce.Consonant}, 介母:{pronounce.SemiVowel}, 韵母: {pronounce.Vowel}, ToString: {pronounce}");
    }
    else
    {
        Console.WriteLine("无效拼音");
    }
}