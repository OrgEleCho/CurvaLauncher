using System.ComponentModel;

namespace CurvaLauncher.Plugins.RunApplication
{
    public enum Win32AppDistinctMode
    {
        [Description("File path")]
        FilePath,

        [Description("File path and description")]
        FilePathAndArguments,
    }
}