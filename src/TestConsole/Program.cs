using CurvaLauncher.Apis;
using CurvaLauncher.Utilities;

var weight1 = StringApi.Instance.Match("Microsoft Edge", "Edge");
var weight2 = StringApi.Instance.Match("Get Help", "Edge");

Console.WriteLine($"Weight1: {weight1}");
Console.WriteLine($"Weight2: {weight2}");
