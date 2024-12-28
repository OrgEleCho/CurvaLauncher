using System.Diagnostics;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Test
{

    public class TestPlugin : SyncPlugin
    {
        [PluginOption]
        public int ResultCount { get; set; } = 5;

        [PluginOption]
        public string Title { get; set; } = "Test";

        [PluginOption]
        public string SomeOption1 { get; set; } = string.Empty;
        [PluginOption]
        public string SomeOption2 { get; set; } = string.Empty;
        [PluginOption]
        public string SomeOption3 { get; set; } = string.Empty;
        [PluginOption]
        public string SomeOption4 { get; set; } = string.Empty;

        [PluginOption]
        public MyFlagEnums SomeOption5 { get; set; } = MyFlagEnums.A;

        public static string IconSvg { get; }
            = "<svg t=\"1695799819564\" class=\"icon\" viewBox=\"0 0 1024 1024\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" p-id=\"1317\" width=\"200\" height=\"200\"><path d=\"M509.53728 510.21824m-473.51808 0a473.51808 473.51808 0 1 0 947.03616 0 473.51808 473.51808 0 1 0-947.03616 0Z\" fill=\"#EEDBC3\" p-id=\"1318\"></path><path d=\"M509.53728 510.21824m-406.46144 0a406.46144 406.46144 0 1 0 812.92288 0 406.46144 406.46144 0 1 0-812.92288 0Z\" fill=\"#78CEF4\" p-id=\"1319\"></path><path d=\"M759.13216 550.53824c-30.91456-49.0752-85.23264-81.92-147.52256-81.92-66.28864 0-123.85792 36.992-153.44128 91.39712h-1.46944c29.42976 54.92224 87.33696 92.30336 154.0096 92.30336 63.13984 0 118.0672-33.76128 148.74112-83.96288a113.4592 113.4592 0 0 0 75.68896 48.37888v-116.0192c-31.81056 5.34016-59.02336 23.8848-76.0064 49.82272z\" fill=\"#FCE38A\" p-id=\"1320\"></path><path d=\"M399.05792 452.89984m-26.19392 0a26.19392 26.19392 0 1 0 52.38784 0 26.19392 26.19392 0 1 0-52.38784 0Z\" fill=\"#FFFFFF\" p-id=\"1321\"></path><path d=\"M298.47552 352.31744m-47.15008 0a47.15008 47.15008 0 1 0 94.30016 0 47.15008 47.15008 0 1 0-94.30016 0Z\" fill=\"#FFFFFF\" p-id=\"1322\"></path><path d=\"M527.2832 547.71712l7.50592-12.99968c7.81312 4.70016 19.36384 8.90368 28.76928 8.90368 9.45664 0 21.33504-4.39296 29.17888-9.14432l7.2192 13.1584c-10.69056 7.03488-23.12192 11.08992-36.4032 11.08992-13.22496-0.00512-25.61024-4.02432-36.27008-11.008z\" fill=\"#5B5144\" p-id=\"1323\"></path></svg>";

        public override ImageSource Icon { get; }

        public override string Name => "Test plugin";
        public override string Description { get; } = "Just Test plugin";


        public TestPlugin(CurvaLauncherContext context) : base(context)
        {
            Icon = context.ImageApi.CreateFromSvg(IconSvg)!;
        }


        public override IEnumerable<IQueryResult> Query(string query)
        {
            for (int i = 0; i < ResultCount - 2; i++)
            {
                yield return new TestQueryResult($"{Title} {i}", Description, (float)i / ResultCount);
            }

            yield return new TestMenuQueryResult();
            yield return new TestDocumentQueryResult();
        }

        public override void Initialize()
        {
            Debug.WriteLine("Plugin loaded");
        }

        public override void Finish()
        {
            Debug.WriteLine("Plugin Unloaded");
        }
    }
}