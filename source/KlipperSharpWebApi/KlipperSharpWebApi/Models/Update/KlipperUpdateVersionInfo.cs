using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AndreasReitberger.Models
{
    public partial class KlipperUpdateVersionInfo
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("debug_enabled")]
        public bool DebugEnabled { get; set; }

        [JsonProperty("need_channel_update")]
        public bool NeedChannelUpdate { get; set; }

        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        [JsonProperty("configured_type")]
        public string ConfiguredType { get; set; }

        [JsonProperty("detected_type")]
        public string DetectedType { get; set; }

        [JsonProperty("remote_alias")]
        public string RemoteAlias { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("repo_name")]
        public string RepoName { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("remote_version")]
        public string RemoteVersion { get; set; }

        [JsonProperty("current_hash")]
        public string CurrentHash { get; set; }

        [JsonProperty("remote_hash")]
        public string RemoteHash { get; set; }

        [JsonProperty("is_dirty")]
        public bool IsDirty { get; set; }

        [JsonProperty("detached")]
        public bool Detached { get; set; }

        [JsonProperty("commits_behind")]
        public List<KlipperUpdateCommitsBehind> CommitsBehind { get; set; } = new();

        [JsonProperty("git_messages")]
        public List<object> GitMessages { get; set; }

        [JsonProperty("full_version_string")]
        public string FullVersionString { get; set; }

        [JsonProperty("pristine")]
        public bool Pristine { get; set; }

        [JsonProperty("package_count")]
        public long PackageCount { get; set; }

        [JsonProperty("package_list")]
        public List<object> PackageList { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
