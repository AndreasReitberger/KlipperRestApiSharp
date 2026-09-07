using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateVersionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("channel")]
        public partial string Channel { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("debug_enabled")]
        public partial bool DebugEnabled { get; set; }

        [ObservableProperty]

        [JsonPropertyName("need_channel_update")]
        public partial bool NeedChannelUpdate { get; set; }

        [ObservableProperty]

        [JsonPropertyName("is_valid")]
        public partial bool IsValid { get; set; }

        [ObservableProperty]

        [JsonPropertyName("configured_type")]
        public partial string ConfiguredType { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("detected_type")]
        public partial string DetectedType { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("remote_alias")]
        public partial string RemoteAlias { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("branch")]
        public partial string Branch { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("owner")]
        public partial string Owner { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("repo_name")]
        public partial string RepoName { get; set; } = string.Empty;

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [JsonPropertyName("version")]
        public partial string Version { get; set; } = string.Empty;

        [ObservableProperty]

        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [JsonPropertyName("remote_version")]
        public partial string RemoteVersion { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("current_hash")]
        public partial string CurrentHash { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("remote_hash")]
        public partial string RemoteHash { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("is_dirty")]
        public partial bool IsDirty { get; set; }

        [ObservableProperty]

        [JsonPropertyName("detached")]
        public partial bool Detached { get; set; }

        [ObservableProperty]

        [JsonPropertyName("commits_behind")]
        public partial List<KlipperUpdateCommitsBehind> CommitsBehind { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("git_messages")]
        public partial List<object> GitMessages { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("full_version_string")]
        public partial string FullVersionString { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("pristine")]
        public partial bool Pristine { get; set; }

        [ObservableProperty]

        [JsonPropertyName("package_count")]
        public partial long PackageCount { get; set; }

        [ObservableProperty]

        [JsonPropertyName("package_list")]
        public partial List<object> PackageList { get; set; } = [];

        bool UpdateAvailable => Version != RemoteVersion;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperUpdateVersionInfo);
        #endregion
    }
}
