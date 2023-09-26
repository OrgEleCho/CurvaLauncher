using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MicaLauncher.Plugin;

namespace MicaLauncher.Services
{
    public class PluginService
    {
        private readonly PathService _pathService;

        public string Path { get; set; } = "Plugins";
        public List<IPlugin> Plugins { get; private set; }

        public PluginService(
            PathService pathService)
        {
            _pathService = pathService;

            LoadPlugins(out var plugins);

            Plugins = plugins;
        }

        private void LoadPlugins(out List<IPlugin> plugins)
        {
            plugins = new List<IPlugin>();

            DirectoryInfo dir = 
                new DirectoryInfo(_pathService.GetPath(Path));

            if (!dir.Exists)
                dir.Create();

            FileInfo[] dllFiles = dir
                .EnumerateFiles()
                .Where(file => file.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach (FileInfo dllFile in dllFiles)
                if (LoadPlugin(dllFile.FullName, out IPlugin? plugin))
                    plugins.Add(plugin);
        }

        private bool LoadPlugin(string dllFilePath, [NotNullWhen(true)] out IPlugin? plugin)
        {
            plugin = null;

            try
            {
                var assembly = Assembly.LoadFile(dllFilePath);

                Type? pluginType = assembly.ExportedTypes
                    .Where(type => type.IsAssignableTo(typeof(IPlugin)))
                    .FirstOrDefault();

                if (pluginType == null)
                    return false;

                plugin = Activator.CreateInstance(pluginType) as IPlugin;

                return plugin != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
