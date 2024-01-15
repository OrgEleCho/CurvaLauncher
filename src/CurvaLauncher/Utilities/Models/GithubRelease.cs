using System;
using System.Text.Json.Serialization;

namespace CurvaLauncher.Utilities.Models;

public record GithubRelease(
    [property: JsonPropertyName("url")] Uri Url,
    [property: JsonPropertyName("assets_url")] Uri AssetsUrl,
    [property: JsonPropertyName("upload_url")] string UploadUrl,
    [property: JsonPropertyName("html_url")] Uri HtmlUrl,
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("author")] Author Author,
    [property: JsonPropertyName("node_id")] string NodeId,
    [property: JsonPropertyName("tag_name")] string TagName,
    [property: JsonPropertyName("target_commitish")] string TargetCommitish,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("draft")] bool Draft,
    [property: JsonPropertyName("prerelease")] bool Prerelease,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("published_at")] DateTimeOffset PublishedAt,
    [property: JsonPropertyName("assets")] Asset[] Assets,
    [property: JsonPropertyName("tarball_url")] Uri TarballUrl,
    [property: JsonPropertyName("zipball_url")] Uri ZipballUrl,
    [property: JsonPropertyName("body")] string Body,
    [property: JsonPropertyName("reactions")] Reactions Reactions
    );

public record Asset(
    [property: JsonPropertyName("url")] Uri Url,
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("node_id")] string NodeId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("label")] object Label,
    [property: JsonPropertyName("uploader")] Author Uploader,
    [property: JsonPropertyName("content_type")] string ContentType,
    [property: JsonPropertyName("state")] string State,
    [property: JsonPropertyName("size")] long Size,
    [property: JsonPropertyName("download_count")] long DownloadCount,
    [property: JsonPropertyName("created_at")] DateTimeOffset CreatedAt,
    [property: JsonPropertyName("updated_at")] DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("browser_download_url")] Uri BrowserDownloadUrl
    );

public record Author(
    [property: JsonPropertyName("login")] string Login,
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("node_id")] string NodeId,
    [property: JsonPropertyName("avatar_url")] Uri AvatarUrl,
    [property: JsonPropertyName("gravatar_id")] string GravatarId,
    [property: JsonPropertyName("url")] Uri Url,
    [property: JsonPropertyName("html_url")] Uri HtmlUrl,
    [property: JsonPropertyName("followers_url")] Uri FollowersUrl,
    [property: JsonPropertyName("following_url")] Uri FollowingUrl,
    [property: JsonPropertyName("gists_url")] string GistsUrl,
    [property: JsonPropertyName("starred_url")] string StarredUrl,
    [property: JsonPropertyName("subscriptions_url")] Uri SubscriptionsUrl,
    [property: JsonPropertyName("organizations_url")] Uri OrganizationsUrl,
    [property: JsonPropertyName("repos_url")] Uri ReposUrl,
    [property: JsonPropertyName("events_url")] Uri EventsUrl,
    [property: JsonPropertyName("received_events_url")] Uri ReceivedEventsUrl,
    [property: JsonPropertyName("type")] string Type,
    [property: JsonPropertyName("site_admin")] bool SiteAdmin);

public record Reactions(
    [property: JsonPropertyName("url")] Uri Url,
    [property: JsonPropertyName("total_count")] long TotalCount,
    [property: JsonPropertyName("+1")] long The1,
    [property: JsonPropertyName("-1")] long Reactions1,
    [property: JsonPropertyName("laugh")] long Laugh,
    [property: JsonPropertyName("hooray")] long Hooray,
    [property: JsonPropertyName("confused")] long Confused,
    [property: JsonPropertyName("heart")] long Heart,
    [property: JsonPropertyName("rocket")] long Rocket,
    [property: JsonPropertyName("eyes")] long Eyes
    );
