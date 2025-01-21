using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateVersionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("channel")]
        public partial string Channel { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("debug_enabled")]
        public partial bool DebugEnabled { get; set; }

        [ObservableProperty]
        
        [JsonProperty("need_channel_update")]
        public partial bool NeedChannelUpdate { get; set; }

        [ObservableProperty]
        
        [JsonProperty("is_valid")]
        public partial bool IsValid { get; set; }

        [ObservableProperty]
        
        [JsonProperty("configured_type")]
        public partial string ConfiguredType { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("detected_type")]
        public partial string DetectedType { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("remote_alias")]
        public partial string RemoteAlias { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("branch")]
        public partial string Branch { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("owner")]
        public partial string Owner { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("repo_name")]
        public partial string RepoName { get; set; } = string.Empty;

        [ObservableProperty]
        
        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [JsonProperty("version")]
        public partial string Version { get; set; } = string.Empty;

        [ObservableProperty]
        
        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [JsonProperty("remote_version")]
        public partial string RemoteVersion { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("current_hash")]
        public partial string CurrentHash { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("remote_hash")]
        public partial string RemoteHash { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("is_dirty")]
        public partial bool IsDirty { get; set; }

        [ObservableProperty]
        
        [JsonProperty("detached")]
        public partial bool Detached { get; set; }

        [ObservableProperty]
        
        [JsonProperty("commits_behind")]
        public partial List<KlipperUpdateCommitsBehind> CommitsBehind { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("git_messages")]
        public partial List<object> GitMessages { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("full_version_string")]
        public partial string FullVersionString { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("pristine")]
        public partial bool Pristine { get; set; }

        [ObservableProperty]
        
        [JsonProperty("package_count")]
        public partial long PackageCount { get; set; }

        [ObservableProperty]
        
        [JsonProperty("package_list")]
        public partial List<object> PackageList { get; set; } = [];

        bool UpdateAvailable => Version != RemoteVersion;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
