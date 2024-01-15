using System;

namespace CurvaLauncher.Services;

public class PathService
{
    public string GetPath(string relativePath)
    {
        return System.IO.Path.Combine(AppContext.BaseDirectory, relativePath);
    }
}
