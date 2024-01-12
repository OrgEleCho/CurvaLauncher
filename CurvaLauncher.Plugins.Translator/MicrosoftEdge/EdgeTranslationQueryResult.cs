using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CurvaLauncher.Plugins.Translator.MicrosoftEdge
{
    public class EdgeTranslationQueryResult : IAsyncQueryResult
    {
        public string Title => "Translate text";

        public string Description => "Translate specified text with 'MicrosoftEdge'";

        public float Weight => 1;

        public ImageSource? Icon => null;

        public string Text { get; }

        public EdgeTranslationQueryResult(HttpClient httpClient, string? sourceLanguage, string? targetLanguage, string text)
        {
            _httpClient = httpClient;
            _sourceLanguage = sourceLanguage;
            _targetLanguage = targetLanguage;
            Text = text;
        }


        static string? s_currentJwt;
        private readonly HttpClient _httpClient;
        private readonly string? _sourceLanguage;
        private readonly string? _targetLanguage;

        async Task<string> GetJwtAsync(CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://edge.microsoft.com/translate/auth"),
                Headers =
                {
                    { "User-Agent", "Edge" }
                }
            };

            var response = await _httpClient.SendAsync(request, cancellationToken);

            return await response.Content.ReadAsStringAsync(cancellationToken);
        }

        bool IsInvalidJwt(string jwt)
        {
            int startIndex = jwt.IndexOf('.');
            int endIndex = jwt.IndexOf('.', startIndex + 1);

            if (startIndex == -1 || endIndex == -1)
                return true;

            string payloadStr = jwt.Substring(startIndex + 1, endIndex - startIndex - 1);

            if (JsonNode.Parse(payloadStr)?["exp"]?.GetValue<long>() is long expTime)
            {
                DateTimeOffset expTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expTime);
                return expTimeOffset < DateTimeOffset.Now;
            }

            return true;
        }

        [MemberNotNull(nameof(s_currentJwt))]
        async Task EnsureJwtAsync(CancellationToken cancellationToken)
        {
            s_currentJwt = "";

            if (string.IsNullOrEmpty(s_currentJwt) || IsInvalidJwt(s_currentJwt))
            {
                s_currentJwt = await GetJwtAsync(cancellationToken);
            }
        }

        async Task<EdgeApiResult> GetTranslationResult(string sourceLanguage, string targetLangauge, string text)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://api.cognitive.microsofttranslator.com/translate?from={Uri.EscapeDataString(sourceLanguage)}&to={Uri.EscapeDataString(targetLangauge)}&api-version=3.0"),
                Headers =
                {
                    { "User-Agent", "Edge" },
                    { "Authorization", $"Bearer {s_currentJwt}" }
                },
                Content = JsonContent.Create(
                    new[]
                    {
                        new
                        {
                            Text = text
                        }
                    })
            };

            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiResult = await response.Content.ReadFromJsonAsync<EdgeApiResult[]>(Converter.Settings);

                return apiResult?.FirstOrDefault() ?? throw new TranslateException("No result");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var errorResult = await response.Content.ReadFromJsonAsync<EdgeApiErrorResult>(Converter.Settings);

                throw new TranslateException(errorResult?.Error?.Message ?? "Unknown error");
            }
            else
            {
                throw new TranslateException($"Unknown error, status code: {(int)response.StatusCode}");
            }
        }


        public async Task InvokeAsync(CancellationToken cancellationToken)
        {
            await EnsureJwtAsync(cancellationToken);

            try
            {
                if (string.IsNullOrWhiteSpace(_targetLanguage) || "Auto".Equals(_targetLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    string currentLangauge = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                    var translationResult = await GetTranslationResult(_sourceLanguage ?? string.Empty, _targetLanguage ?? currentLangauge, Text);

                    if (translationResult == null)
                        return;

                    if (!translationResult.DetectedLanguage.Language.StartsWith(currentLangauge))
                    {
                        // right result
                        if (translationResult.Translations != null && translationResult.Translations.FirstOrDefault() is Translation rightTranslation)
                            Clipboard.SetText(rightTranslation.Text);

                        return;
                    }

                    // wrong result
                    string realTargetLanguage;
                    if (currentLangauge == "en")
                        realTargetLanguage = "zh";
                    else
                        realTargetLanguage = "en";

                    translationResult = await GetTranslationResult(_sourceLanguage ?? string.Empty, realTargetLanguage, Text);
                    if (translationResult?.Translations != null && translationResult.Translations.FirstOrDefault() is Translation fallbackTranslation)
                        Clipboard.SetText(fallbackTranslation.Text);
                }
                else
                {
                    var translationResult = await GetTranslationResult(_sourceLanguage ?? string.Empty, _targetLanguage, Text);
                    if (translationResult?.Translations != null && translationResult.Translations.FirstOrDefault() is Translation translation)
                        Clipboard.SetText(translation.Text);
                }
            }
            catch (TranslateException ex)
            {
                MessageBox.Show($"{ex.Message}", "Translate failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
