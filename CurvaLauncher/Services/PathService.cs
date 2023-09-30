using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurvaLauncher.Services
{
    public class PathService
    {
        public string GetPath(string relativePath)
        {
            return System.IO.Path.Combine(AppContext.BaseDirectory, relativePath);
        }
    }
}
