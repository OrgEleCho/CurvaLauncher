using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Plugins.Translator.MicrosoftEdge
{
    using System;
    using System.Collections.Generic;

    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Globalization;

    public partial class EdgeApiResult
    {
        public EdgeApiResult(DetectedLanguage detectedLanguage, Translation[] translations)
        {
            DetectedLanguage = detectedLanguage;
            Translations = translations;
        }

        [JsonPropertyName("detectedLanguage")]
        public DetectedLanguage DetectedLanguage { get; set; }

        [JsonPropertyName("translations")]
        public Translation[] Translations { get; set; }
    }


    public partial class EdgeApiErrorResult
    {
        public EdgeApiErrorResult(Error error)
        {
            Error = error;
        }

        [JsonPropertyName("error")]
        public Error Error { get; set; }
    }

    public partial class Error
    {
        public Error(long code, string message)
        {
            Code = code;
            Message = message;
        }

        [JsonPropertyName("code")]
        public long Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public partial class DetectedLanguage
    {
        public DetectedLanguage(string language, double score)
        {
            Language = language;
            Score = score;
        }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }
    }

    public partial class Translation
    {
        public Translation(string text, string to, SentLen sentLen)
        {
            Text = text;
            To = to;
            SentLen = sentLen;
        }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("to")]
        public string To { get; set; }

        [JsonPropertyName("sentLen")]
        public SentLen? SentLen { get; set; }
    }

    public partial class SentLen
    {
        public SentLen(long[] srcSentLen, long[] transSentLen)
        {
            SrcSentLen = srcSentLen;
            TransSentLen = transSentLen;
        }

        [JsonPropertyName("srcSentLen")]
        public long[] SrcSentLen { get; set; }

        [JsonPropertyName("transSentLen")]
        public long[] TransSentLen { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
        {
            Converters =
            {

            },
        };
    }
}
