using System.Net.Http.Json;
using System.Windows;
using System.Windows.Media;
using CurvaLauncher.Data;

namespace CurvaLauncher.Plugin.Translator.Youdao
{
    public class YoudaoTranslationQueryResult : AsyncQueryResult
    {
        private readonly TranslatorPlugin _plugin;
        private readonly string? _sourceLanguage;
        private readonly string? _targetLanguage;

        public override float Weight => 1;

        public override string Title => "Translate text";

        public override string Description => "Translate specified text with 'Youdao'";

        public override ImageSource? Icon => null;

        public string Text { get; }

        public YoudaoTranslationQueryResult(TranslatorPlugin plugin, string? sourceLanguage, string? targetLanguage, string text)
        {
            _plugin = plugin;
            _sourceLanguage = sourceLanguage;
            _targetLanguage = targetLanguage;
            Text = text;
        }

        public override async Task InvokeAsync(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://aidemo.youdao.com/trans"),
                Content = new FormUrlEncodedContent(
                    new Dictionary<string, string>()
                    {
                        ["from"] = _sourceLanguage ?? "auto",
                        ["to"] = _targetLanguage ?? "auto",
                        ["q"] = Text
                    })
            };

            var response = await _plugin.HttpClient.SendAsync(request, cancellationToken);
            var result = await response.Content.ReadFromJsonAsync<YoudaoApiResult>(Converter.Settings, cancellationToken);

            if (result?.Translation != null && result.Translation.Any())
                Clipboard.SetText(result.Translation[0]);
        }
    }
}
