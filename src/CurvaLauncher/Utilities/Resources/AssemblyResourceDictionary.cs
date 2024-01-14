using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CurvaLauncher.Utilities.Resources
{
    class AssemblyResourceDictionary : ResourceDictionary
    {
        public AssemblyResourceDictionary(Assembly assembly, ResourceDictionary targetResourceDictionary)
        {
            Assembly = assembly;
            TargetResourceDictionary = targetResourceDictionary;

            foreach (var key in targetResourceDictionary.Keys)
                Add(new AssemblyResourceKey(assembly, key), targetResourceDictionary[key]);
        }

        public Assembly Assembly { get; }
        public ResourceDictionary TargetResourceDictionary { get; }
    }
}
