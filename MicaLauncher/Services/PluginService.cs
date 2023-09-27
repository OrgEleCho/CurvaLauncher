using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MicaLauncher.Models;
using MicaLauncher.Plugin;

namespace MicaLauncher.Services
{
    public class PluginService
    {
        private readonly PathService _pathService;

        public string Path { get; set; } = "Plugins";
        public List<MicaLauncherPluginInstance> PluginInstances { get; private set; }

        public PluginService(
            PathService pathService)
        {
            _pathService = pathService;

            LoadPlugins(out var plugins);

            PluginInstances = plugins;
        }

        private void LoadPlugins(out List<MicaLauncherPluginInstance> plugins)
        {
            plugins = new List<MicaLauncherPluginInstance>();

            DirectoryInfo dir = 
                new DirectoryInfo(_pathService.GetPath(Path));

            if (!dir.Exists)
                dir.Create();

            FileInfo[] dllFiles = dir
                .EnumerateFiles()
                .Where(file => file.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            foreach (FileInfo dllFile in dllFiles)
                if (LoadPlugin(dllFile.FullName, out MicaLauncherPluginInstance? plugin))
                    plugins.Add(plugin);
        }

        private bool LoadPlugin(string dllFilePath, [NotNullWhen(true)] out MicaLauncherPluginInstance? plugin)
        {
            plugin = null;

            try
            {
                var assembly = Assembly.LoadFile(dllFilePath);

                Type? pluginType = assembly.ExportedTypes
                    .Where(type => type.IsAssignableTo(typeof(ISyncPlugin)))
                    .FirstOrDefault();

                if (pluginType == null)
                    return false;

                if (MicaLauncherPluginInstance.TryCreate(pluginType, out plugin))
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
