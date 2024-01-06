using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using CurvaLauncher.Models;
using CurvaLauncher.Plugin;

namespace CurvaLauncher.Services;

public class PluginService
{
    private readonly PathService _pathService;

    public string Path { get; set; } = "Plugins";
    public List<CurvaLauncherPluginInstance> PluginInstances { get; private set; }

    public PluginService(
        PathService pathService)
    {
        _pathService = pathService;

        LoadPlugins(out var plugins);

        PluginInstances = plugins;
    }

    private void LoadPlugins(out List<CurvaLauncherPluginInstance> plugins)
    {
        plugins = new List<CurvaLauncherPluginInstance>();

        DirectoryInfo dir = 
            new DirectoryInfo(_pathService.GetPath(Path));

        if (!dir.Exists)
        {
            dir.Create();
            return;
        }

        var dllFiles = dir.GetFiles("*.dll");

//#if DEBUG
//            var debugDir = Directory.GetCurrentDirectory();
//            var projectRoot = System.IO.Path.Combine(debugDir, "..", "..", "..", "..");
//            var debugDll1 = System.IO.Path.Combine(projectRoot, "CurvaLauncher.Plugin.RunApplication", "bin", "Debug", "net6.0-windows");
//            var debugDll2 = System.IO.Path.Combine(projectRoot, "CurvaLauncher.Plugin.RunProgram", "bin", "Debug", "net6.0-windows");

//            foreach (var debugDll in Directory.GetFiles(debugDll1, "*.dll").Concat(Directory.GetFiles(debugDll2, "*.dll")))
//                if (LoadPlugin(debugDll, out CurvaLauncherPluginInstance? plugin))
//                    plugins.Add(plugin);
//#endif

        foreach (FileInfo dllFile in dllFiles)
            if (LoadPlugin(dllFile.FullName, out CurvaLauncherPluginInstance? plugin))
                plugins.Add(plugin);
    }

    private bool LoadPlugin(string dllFilePath, [NotNullWhen(true)] out CurvaLauncherPluginInstance? plugin)
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

            if (CurvaLauncherPluginInstance.TryCreate(pluginType, out plugin))
                return true;

            return false;
        }
        catch
        {
            return false;
        }
    }
}
