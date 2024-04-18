using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateVersionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("channel")]
        string channel = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("debug_enabled")]
        bool debugEnabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("need_channel_update")]
        bool needChannelUpdate;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("is_valid")]
        bool isValid;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("configured_type")]
        string configuredType = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("detected_type")]
        string detectedType = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("remote_alias")]
        string remoteAlias = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("branch")]
        string branch = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("owner")]
        string owner = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("repo_name")]
        string repoName = string.Empty;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [property: JsonProperty("version")]
        string version = string.Empty;

        [ObservableProperty, JsonIgnore]
        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [property: JsonProperty("remote_version")]
        string remoteVersion = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("current_hash")]
        string currentHash = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("remote_hash")]
        string remoteHash = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("is_dirty")]
        bool isDirty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("detached")]
        bool detached;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("commits_behind")]
        List<KlipperUpdateCommitsBehind> commitsBehind = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("git_messages")]
        List<object> gitMessages = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("full_version_string")]
        string fullVersionString = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pristine")]
        bool pristine;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("package_count")]
        long packageCount;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("package_list")]
        List<object> packageList = [];

        bool UpdateAvailable => Version != RemoteVersion;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
