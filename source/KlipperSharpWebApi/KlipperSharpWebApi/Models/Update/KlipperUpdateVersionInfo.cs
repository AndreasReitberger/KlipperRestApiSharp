using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperUpdateVersionInfo
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("channel")]
        public string Channel { get; set; } = string.Empty;

        [JsonProperty("debug_enabled")]
        public bool DebugEnabled { get; set; }

        [JsonProperty("need_channel_update")]
        public bool NeedChannelUpdate { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("configured_type")]
        public string ConfiguredType { get; set; } = string.Empty;

        [JsonProperty("detected_type")]
        public string DetectedType { get; set; } = string.Empty;

        [JsonProperty("remote_alias")]
        public string RemoteAlias { get; set; } = string.Empty;

        [JsonProperty("branch")]
        public string Branch { get; set; } = string.Empty;

        [JsonProperty("owner")]
        public string Owner { get; set; } = string.Empty;

        [JsonProperty("repo_name")]
        public string RepoName { get; set; } = string.Empty;

        [JsonProperty("version")]
        public string Version { get; set; } = string.Empty;

        [JsonProperty("remote_version")]
        public string RemoteVersion { get; set; } = string.Empty;

        [JsonProperty("current_hash")]
        public string CurrentHash { get; set; } = string.Empty;

        [JsonProperty("remote_hash")]
        public string RemoteHash { get; set; } = string.Empty;

        [JsonProperty("is_dirty")]
        public bool IsDirty { get; set; }

        [JsonProperty("detached")]
        public bool Detached { get; set; }

        [JsonProperty("commits_behind")]
        public List<KlipperUpdateCommitsBehind> CommitsBehind { get; set; } = new();

        [JsonProperty("git_messages")]
        public List<object> GitMessages { get; set; } = new();

        [JsonProperty("full_version_string")]
        public string FullVersionString { get; set; } = string.Empty;

        [JsonProperty("pristine")]
        public bool Pristine { get; set; }

        [JsonProperty("package_count")]
        public long PackageCount { get; set; }

        [JsonProperty("package_list")]
        public List<object> PackageList { get; set; } = new();

        public bool UpdateAvailable => Version != RemoteVersion;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
