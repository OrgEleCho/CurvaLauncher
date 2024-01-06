using System.Text.Encodings.Web;
using System.Text.Json;

namespace CurvaLauncher.Utilities;

internal class JsonUtils
{
    public static JsonSerializerOptions Options { get; } = new JsonSerializerOptions()
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters =
        {
            
        }
    };
}
