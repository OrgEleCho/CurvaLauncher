using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace CurvaLauncher.Plugins;

public sealed class PluginAssemblyLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver resolver;

    public PluginAssemblyLoadContext(string pluginID, string dllPath)
        : base($"ZipPlugin ALC - {pluginID}")
    {
        resolver = new(dllPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string? resolvedPath = resolver.ResolveAssemblyToPath(assemblyName);
        return resolvedPath is not null ? LoadFromAssemblyPath(resolvedPath) : null;
    }

    protected override nint LoadUnmanagedDll(string unmanagedDllName)
    {
        string? resolvedPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return resolvedPath is not null ? LoadUnmanagedDllFromPath(resolvedPath) : IntPtr.Zero;
    }
}
