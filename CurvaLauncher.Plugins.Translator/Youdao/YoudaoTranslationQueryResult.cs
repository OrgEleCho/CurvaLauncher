using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Translator.Youdao
{
    public class YoudaoTranslationQueryResult : IAsyncQueryResult
    {
        private readonly HttpClient _httpClient;
        private readonly string? _sourceLanguage;
        private readonly string? _targetLanguage;

        public float Weight => 1;

        public string Title => "Translate text";

        public string Description => "Translate specified text with 'Youdao'";

        public ImageSource? Icon => null;

        public string Text { get; }

        public YoudaoTranslationQueryResult(HttpClient httpClient, string? sourceLanguage, string? targetLanguage, string text)
        {
            _httpClient = httpClient;
            _sourceLanguage = sourceLanguage;
            _targetLanguage = targetLanguage;
            Text = text;
        }

        public async Task InvokeAsync(CancellationToken cancellationToken)
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

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var result = await response.Content.ReadFromJsonAsync<YoudaoApiResult>(Converter.Settings, cancellationToken);

            if (result?.Translation != null && result.Translation.Any())
                Clipboard.SetText(result.Translation[0]);
        }
    }
}
