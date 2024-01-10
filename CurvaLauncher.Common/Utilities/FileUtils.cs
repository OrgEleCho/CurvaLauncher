using System.IO;
using CurvaLauncher.Libraries.Securify.ShellLink.Flags;

namespace CurvaLauncher.Utilities
{
    public static class FileUtils
    {
        public record class ShortcutTarget(string FileName, string? Arguments, string? WorkingDirectory, string? IconPath, int IconIndex, bool UAC);

        public static ShortcutTarget? GetShortcutTarget(string file)
        {
            try
			{
                var shortcut = Libraries.Securify.ShellLink.Shortcut.ReadFromFile(file);

                var fileName =
                    shortcut.ExtraData?.EnvironmentVariableDataBlock?.TargetUnicode ??
                    shortcut.ExtraData?.EnvironmentVariableDataBlock?.TargetAnsi ??
                    shortcut.LinkTargetIDList?.Path ?? 
                    shortcut.LinkTargetIDList?.DisplayName;

                if (fileName == null)
                    return null;

                var commandLineArguments = shortcut.LinkFlags.HasFlag(LinkFlags.HasArguments) ?
                    shortcut.StringData?.CommandLineArguments :
                    null;

                var workingDirectory = shortcut.StringData?.WorkingDir;

                var iconPath = shortcut.StringData?.IconLocation;
                var iconIndex = shortcut.IconIndex;

                var uac = shortcut.LinkFlags.HasFlag(LinkFlags.RunAsUser);

                return new ShortcutTarget(fileName, commandLineArguments, workingDirectory, iconPath, iconIndex, uac);
            }
			catch (Exception)
			{
                return null;
			}
        }
    }
}
