using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Services
{
    public class LibraryService
    {
        private readonly PathService _pathService;

        public string Path { get; set; } = "Libraries";

        public ObservableCollection<Assembly> Libraries { get; } = new();

        public LibraryService(
            PathService pathService)
        {
            _pathService = pathService;
        }

        private DirectoryInfo EnsureLibrariesDirectory()
        {
            DirectoryInfo dir = new DirectoryInfo(_pathService.GetPath(Path));

            if (!dir.Exists)
                dir.Create();

            return dir;
        }

        public void Setup()
        {
            var dir = EnsureLibrariesDirectory();
            var dllFiles = dir.EnumerateFiles("*.dll");

            NativeLibrary.SetDllImportResolver()

            foreach (var dllFile in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFile(dllFile.FullName);
                    Libraries.Add(assembly);
                }
                catch
                {

                }
            }
        }
    }
}
