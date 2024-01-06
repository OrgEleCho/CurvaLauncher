using System.IO;

namespace CurvaLauncher.Utilities
{
    public static class FileUtils
    {
        public static string? GetShortcutTarget(string file)
        {
			try
			{
                var shortcut = Libraries.Securify.ShellLink.Shortcut.ReadFromFile(file);

                return
                    shortcut.ExtraData?.EnvironmentVariableDataBlock?.TargetUnicode ??
                    shortcut.ExtraData?.EnvironmentVariableDataBlock?.TargetAnsi ??
                    shortcut.LinkTargetIDList?.Path ?? 
                    shortcut.LinkTargetIDList?.DisplayName;
            }
			catch (Exception)
			{
                return null;
			}
        }
    }
}
