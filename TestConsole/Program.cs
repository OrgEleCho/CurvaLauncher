using CurvaLauncher.Utilities;

SortedCollection<int, int> qwq = new();

qwq.SortingRoot = v => v;

for (int i = 0; i < 10; i++)
{
    qwq.Add(Random.Shared.Next(10));
}

Console.WriteLine($"[{string.Join(", ", qwq)}]");