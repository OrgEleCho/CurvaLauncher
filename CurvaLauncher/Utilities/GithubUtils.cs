using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using CurvaLauncher.Utilities.Models;

namespace CurvaLauncher.Utilities;

static class GithubUtils
{
    static HttpClient s_httpClient = new();

    public static async Task<(Version Version, string Address)?> GetLatestVersionAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/repos/OrgEleCho/CurvaLauncher/releases/latest")
        {
            Headers =
            {
                { "Host", "api.github.com" },
                { "User-Agent", $"CurvaLauncher/{App.Version}" }
            }
        };
        var response = await s_httpClient.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            return null;
        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            throw new HttpRequestException();

        var release = await response.Content.ReadFromJsonAsync<GithubRelease>();

        if (release == null)
            return null;

        var tag = release.TagName;
        var versionStr = tag.TrimStart('v');

        if (!Version.TryParse(versionStr, out var version))
            return null;

        return (version, release.AssetsUrl.ToString());
    }
}