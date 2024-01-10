using System.ComponentModel;

namespace CurvaLauncher.Models;

public enum AppLanguage
{
    Auto,
    English,

    [Description("简体中文")]
    ChineseSimplified,

    [Description("繁体中文")]
    ChineseTraditional,
    Japanese
}
