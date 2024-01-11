using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Plugins.Translator.Youdao
{
    using System;
    using System.Collections.Generic;

    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Globalization;

    public partial class YoudaoApiResult
    {
        public YoudaoApiResult(string[] returnPhrase, string query, long errorCode, string l, Uri tSpeakUrl, Web[] web, Guid requestId, string[] translation, Dict mTerminalDict, Dict dict, Dict webdict, Basic basic, bool isWord, Uri speakUrl)
        {
            ReturnPhrase = returnPhrase;
            Query = query;
            ErrorCode = errorCode;
            L = l;
            TSpeakUrl = tSpeakUrl;
            Web = web;
            RequestId = requestId;
            Translation = translation;
            MTerminalDict = mTerminalDict;
            Dict = dict;
            Webdict = webdict;
            Basic = basic;
            IsWord = isWord;
            SpeakUrl = speakUrl;
        }

        [JsonPropertyName("returnPhrase")]
        public string[] ReturnPhrase { get; set; }

        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("errorCode")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long ErrorCode { get; set; }

        [JsonPropertyName("l")]
        public string L { get; set; }

        [JsonPropertyName("tSpeakUrl")]
        public Uri TSpeakUrl { get; set; }

        [JsonPropertyName("web")]
        public Web[] Web { get; set; }

        [JsonPropertyName("requestId")]
        public Guid RequestId { get; set; }

        [JsonPropertyName("translation")]
        public string[] Translation { get; set; }

        [JsonPropertyName("mTerminalDict")]
        public Dict MTerminalDict { get; set; }

        [JsonPropertyName("dict")]
        public Dict Dict { get; set; }

        [JsonPropertyName("webdict")]
        public Dict Webdict { get; set; }

        [JsonPropertyName("basic")]
        public Basic Basic { get; set; }

        [JsonPropertyName("isWord")]
        public bool IsWord { get; set; }

        [JsonPropertyName("speakUrl")]
        public Uri SpeakUrl { get; set; }
    }

    public partial class Basic
    {
        public Basic(string[] examType, string usPhonetic, string phonetic, string ukPhonetic, WfElement[] wfs, Uri ukSpeech, string[] explains, Uri usSpeech)
        {
            ExamType = examType;
            UsPhonetic = usPhonetic;
            Phonetic = phonetic;
            UkPhonetic = ukPhonetic;
            Wfs = wfs;
            UkSpeech = ukSpeech;
            Explains = explains;
            UsSpeech = usSpeech;
        }

        [JsonPropertyName("exam_type")]
        public string[] ExamType { get; set; }

        [JsonPropertyName("us-phonetic")]
        public string UsPhonetic { get; set; }

        [JsonPropertyName("phonetic")]
        public string Phonetic { get; set; }

        [JsonPropertyName("uk-phonetic")]
        public string UkPhonetic { get; set; }

        [JsonPropertyName("wfs")]
        public WfElement[] Wfs { get; set; }

        [JsonPropertyName("uk-speech")]
        public Uri UkSpeech { get; set; }

        [JsonPropertyName("explains")]
        public string[] Explains { get; set; }

        [JsonPropertyName("us-speech")]
        public Uri UsSpeech { get; set; }
    }

    public partial class WfElement
    {
        public WfElement(WfWf wf)
        {
            Wf = wf;
        }

        [JsonPropertyName("wf")]
        public WfWf Wf { get; set; }
    }

    public partial class WfWf
    {
        public WfWf(string name, string value)
        {
            Name = name;
            Value = value;
        }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public partial class Dict
    {
        public Dict(string url)
        {
            Url = url;
        }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public partial class Web
    {
        public Web(string[] value, string key)
        {
            Value = value;
            Key = key;
        }

        [JsonPropertyName("value")]
        public string[] Value { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerOptions Settings = new(JsonSerializerDefaults.General)
        {
            Converters =
            {
                new DateOnlyConverter(),
                new TimeOnlyConverter(),
                IsoDateTimeOffsetConverter.Singleton
            },
        };
    }

    internal class ParseStringConverter : JsonConverter<long>
    {
        public override bool CanConvert(Type t) => t == typeof(long);

        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value.ToString(), options);
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string serializationFormat;
        public DateOnlyConverter() : this(null) { }

        public DateOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "yyyy-MM-dd";
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }

    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        private readonly string serializationFormat;

        public TimeOnlyConverter() : this(null) { }

        public TimeOnlyConverter(string? serializationFormat)
        {
            this.serializationFormat = serializationFormat ?? "HH:mm:ss.fff";
        }

        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return TimeOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(serializationFormat));
    }

    internal class IsoDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override bool CanConvert(Type t) => t == typeof(DateTimeOffset);

        private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";

        private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
        private string? _dateTimeFormat;
        private CultureInfo? _culture;

        public DateTimeStyles DateTimeStyles
        {
            get => _dateTimeStyles;
            set => _dateTimeStyles = value;
        }

        public string? DateTimeFormat
        {
            get => _dateTimeFormat ?? string.Empty;
            set => _dateTimeFormat = (string.IsNullOrEmpty(value)) ? null : value;
        }

        public CultureInfo Culture
        {
            get => _culture ?? CultureInfo.CurrentCulture;
            set => _culture = value;
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            string text;


            if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
                || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
            {
                value = value.ToUniversalTime();
            }

            text = value.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);

            writer.WriteStringValue(text);
        }

        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? dateText = reader.GetString();

            if (string.IsNullOrEmpty(dateText) == false)
            {
                if (!string.IsNullOrEmpty(_dateTimeFormat))
                {
                    return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
                }
                else
                {
                    return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
                }
            }
            else
            {
                return default(DateTimeOffset);
            }
        }


        public static readonly IsoDateTimeOffsetConverter Singleton = new IsoDateTimeOffsetConverter();
    }
}
