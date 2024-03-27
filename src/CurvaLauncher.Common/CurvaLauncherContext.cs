using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using CurvaLauncher.Apis;

namespace CurvaLauncher
{
    public abstract class CurvaLauncherContext
    {
        public abstract Dispatcher Dispatcher { get; }

        /// <summary>
        /// Required query result icon size
        /// </summary>
        public abstract int RequiredIconSize { get; }

        public abstract CultureInfo CurrentCulture { get; }


        public abstract ICommonApi Api { get; }
        public abstract IFileApi FileApi { get; }
        public abstract IImageApi ImageApi { get; }
        public abstract IStringApi StringApi { get; }
        public abstract IClipboardApi ClipboardApi { get; }
        public abstract ICommandLineApi CommandLineApi { get; }



        public abstract bool IsShiftKeyPressed();
        public abstract bool IsCtrlKeyPressed();
        public abstract bool IsAltKeyPressed();

        public abstract void AddI18nResourceDictionary(Assembly assembly, CultureInfo cultureInfo, ResourceDictionary resourceDictionary);
        public abstract object GetI18nResourceValue(Assembly assembly, object? key);
        public abstract string? GetI18nResourceString(Assembly assembly, object? key);


        public abstract event EventHandler? AppLanguageChanged;
    }
}
