using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CurvaLauncher.Utilities.Models
{
    public partial class GithubRelease
    {
        public GithubRelease(
            Uri url,
            Uri assetsUrl,
            string uploadUrl,
            Uri htmlUrl,
            long id,
            Author author,
            string nodeId,
            string tagName,
            string targetCommitish,
            string name,
            bool draft,
            bool prerelease,
            DateTimeOffset createdAt,
            DateTimeOffset publishedAt,
            Asset[] assets,
            Uri tarballUrl,
            Uri zipballUrl,
            string body,
            Reactions reactions)
        {
            Url = url;
            AssetsUrl = assetsUrl;
            UploadUrl = uploadUrl;
            HtmlUrl = htmlUrl;
            Id = id;
            Author = author;
            NodeId = nodeId;
            TagName = tagName;
            TargetCommitish = targetCommitish;
            Name = name;
            Draft = draft;
            Prerelease = prerelease;
            CreatedAt = createdAt;
            PublishedAt = publishedAt;
            Assets = assets;
            TarballUrl = tarballUrl;
            ZipballUrl = zipballUrl;
            Body = body;
            Reactions = reactions;
        }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("assets_url")]
        public Uri AssetsUrl { get; set; }

        [JsonPropertyName("upload_url")]
        public string UploadUrl { get; set; }

        [JsonPropertyName("html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }

        [JsonPropertyName("tag_name")]
        public string TagName { get; set; }

        [JsonPropertyName("target_commitish")]
        public string TargetCommitish { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("draft")]
        public bool Draft { get; set; }

        [JsonPropertyName("prerelease")]
        public bool Prerelease { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("published_at")]
        public DateTimeOffset PublishedAt { get; set; }

        [JsonPropertyName("assets")]
        public Asset[] Assets { get; set; }

        [JsonPropertyName("tarball_url")]
        public Uri TarballUrl { get; set; }

        [JsonPropertyName("zipball_url")]
        public Uri ZipballUrl { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }

        [JsonPropertyName("reactions")]
        public Reactions Reactions { get; set; }
    }

    public partial class Asset
    {
        public Asset(Uri url,
            long id,
            string nodeId,
            string name,
            object label,
            Author uploader,
            string contentType,
            string state,
            long size,
            long downloadCount,
            DateTimeOffset createdAt,
            DateTimeOffset updatedAt,
            Uri browserDownloadUrl)
        {
            Url = url;
            Id = id;
            NodeId = nodeId;
            Name = name;
            Label = label;
            Uploader = uploader;
            ContentType = contentType;
            State = state;
            Size = size;
            DownloadCount = downloadCount;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            BrowserDownloadUrl = browserDownloadUrl;
        }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public object Label { get; set; }

        [JsonPropertyName("uploader")]
        public Author Uploader { get; set; }

        [JsonPropertyName("content_type")]
        public string ContentType { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("download_count")]
        public long DownloadCount { get; set; }

        [JsonPropertyName("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonPropertyName("browser_download_url")]
        public Uri BrowserDownloadUrl { get; set; }
    }

    public partial class Author
    {
        public Author(
            string login,
            long id,
            string nodeId,
            Uri avatarUrl,
            string gravatarId,
            Uri url,
            Uri htmlUrl,
            Uri followersUrl,
            string followingUrl,
            string gistsUrl,
            string starredUrl,
            Uri subscriptionsUrl,
            Uri organizationsUrl,
            Uri reposUrl,
            string eventsUrl,
            Uri receivedEventsUrl,
            string type,
            bool siteAdmin)
        {
            Login = login;
            Id = id;
            NodeId = nodeId;
            AvatarUrl = avatarUrl;
            GravatarId = gravatarId;
            Url = url;
            HtmlUrl = htmlUrl;
            FollowersUrl = followersUrl;
            FollowingUrl = followingUrl;
            GistsUrl = gistsUrl;
            StarredUrl = starredUrl;
            SubscriptionsUrl = subscriptionsUrl;
            OrganizationsUrl = organizationsUrl;
            ReposUrl = reposUrl;
            EventsUrl = eventsUrl;
            ReceivedEventsUrl = receivedEventsUrl;
            Type = type;
            SiteAdmin = siteAdmin;
        }

        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }

        [JsonPropertyName("avatar_url")]
        public Uri AvatarUrl { get; set; }

        [JsonPropertyName("gravatar_id")]
        public string GravatarId { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonPropertyName("followers_url")]
        public Uri FollowersUrl { get; set; }

        [JsonPropertyName("following_url")]
        public string FollowingUrl { get; set; }

        [JsonPropertyName("gists_url")]
        public string GistsUrl { get; set; }

        [JsonPropertyName("starred_url")]
        public string StarredUrl { get; set; }

        [JsonPropertyName("subscriptions_url")]
        public Uri SubscriptionsUrl { get; set; }

        [JsonPropertyName("organizations_url")]
        public Uri OrganizationsUrl { get; set; }

        [JsonPropertyName("repos_url")]
        public Uri ReposUrl { get; set; }

        [JsonPropertyName("events_url")]
        public string EventsUrl { get; set; }

        [JsonPropertyName("received_events_url")]
        public Uri ReceivedEventsUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("site_admin")]
        public bool SiteAdmin { get; set; }
    }

    public partial class Reactions
    {
        public Reactions(
            Uri url,
            long totalCount,
            long the1,
            long reactions1,
            long laugh,
            long hooray,
            long confused,
            long heart,
            long rocket,
            long eyes)
        {
            Url = url;
            TotalCount = totalCount;
            The1 = the1;
            Reactions1 = reactions1;
            Laugh = laugh;
            Hooray = hooray;
            Confused = confused;
            Heart = heart;
            Rocket = rocket;
            Eyes = eyes;
        }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("total_count")]
        public long TotalCount { get; set; }

        [JsonPropertyName("+1")]
        public long The1 { get; set; }

        [JsonPropertyName("-1")]
        public long Reactions1 { get; set; }

        [JsonPropertyName("laugh")]
        public long Laugh { get; set; }

        [JsonPropertyName("hooray")]
        public long Hooray { get; set; }

        [JsonPropertyName("confused")]
        public long Confused { get; set; }

        [JsonPropertyName("heart")]
        public long Heart { get; set; }

        [JsonPropertyName("rocket")]
        public long Rocket { get; set; }

        [JsonPropertyName("eyes")]
        public long Eyes { get; set; }
    }
}
