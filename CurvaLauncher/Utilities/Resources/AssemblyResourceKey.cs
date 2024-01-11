using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace CurvaLauncher.Utilities.Resources
{
    public class AssemblyResourceKey : ResourceKey, IEquatable<AssemblyResourceKey?>
    {
        public AssemblyResourceKey(Assembly assembly, object? key)
        {
            Assembly = assembly;
            Key = key;
        }

        public override Assembly Assembly { get; }
        public object? Key { get; }

        public override bool Equals(object? obj) => Equals(obj as AssemblyResourceKey);
        public bool Equals(AssemblyResourceKey? other) => other is not null && EqualityComparer<Assembly>.Default.Equals(Assembly, other.Assembly) && EqualityComparer<object?>.Default.Equals(Key, other.Key);
        public override int GetHashCode() => HashCode.Combine(Assembly, Key);

        public static bool operator ==(AssemblyResourceKey? left, AssemblyResourceKey? right) => EqualityComparer<AssemblyResourceKey>.Default.Equals(left, right);
        public static bool operator !=(AssemblyResourceKey? left, AssemblyResourceKey? right) => !(left == right);
    }
}
