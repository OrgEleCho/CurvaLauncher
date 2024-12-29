using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CurvaLauncher;
using CurvaLauncher.Plugins;

namespace CurvaLauncher.PluginTemplate
{
#if (!EnableI18n)
    public class SomePlugin : Plugin, ISyncPlugin
    {
        public override string Name => "Sample Plugin";                               // Plugin Name
        public override string Description => "Plugin Description";                   // Plugin Description

        public override ImageSource Icon { get; }
            = new WriteableBitmap(10, 10, 96, 96, PixelFormats.Bgra32, null);         // Empty Icon

        public SomePlugin(CurvaLauncherContext context) : base(context)
        {
            // With CurvaLauncherContext, you can use some APIs
            // provided by CurvaLauncher

        }

        public IEnumerable<IQueryResult> Query(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                yield break;
            }

            yield return new EmptyQueryResult("Test Plugin OK", $"Your plugin {Name} is running well", 1, null);
        }
    }
#else
    public class SomePlugin : SyncI18nPlugin
    {
        private readonly CurvaLauncherContext _context;
        private readonly Assembly _currentAssembly;

        public override object NameKey => "StrPluginName";                            // Plugin name resource key
        public override object DescriptionKey => "StrPluginDescription";              // Plugin description resource key

        public override ImageSource Icon { get; }
            = new WriteableBitmap(10, 10, 96, 96, PixelFormats.Bgra32, null);         // Empty Icon

        public SomePlugin(CurvaLauncherContext context) : base(context)
        {
            _context = context;
            _currentAssembly = Assembly.GetExecutingAssembly();

            // With CurvaLauncherContext, you can use some APIs
            // provided by CurvaLauncher

        }

        public override IEnumerable<I18nResourceDictionary> GetI18nResourceDictionaries()
        {
            yield return I18nResourceDictionary.Create(new CultureInfo("en-US"), "I18n/EnUs.xaml");
            yield return I18nResourceDictionary.Create(new CultureInfo("zh-Hans"), "I18n/ZhHans.xaml");
        }

        public override IEnumerable<IQueryResult> Query(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                yield break;
            }

            yield return new EmptyQueryResult(
                _context.GetI18nResourceString(_currentAssembly, "StrTestResultTitle")!,
                _context.GetI18nResourceString(_currentAssembly, "StrTestResultDescription")!,
                1,
                null);
        }
    }
#endif
}
