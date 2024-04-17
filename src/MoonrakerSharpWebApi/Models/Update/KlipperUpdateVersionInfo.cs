using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateVersionInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("channel")]
        string channel = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("debug_enabled")]
        bool debugEnabled;

        [ObservableProperty]
        [property: JsonProperty("need_channel_update")]
        bool needChannelUpdate;

        [ObservableProperty]
        [property: JsonProperty("is_valid")]
        bool isValid;

        [ObservableProperty]
        [property: JsonProperty("configured_type")]
        string configuredType = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("detected_type")]
        string detectedType = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("remote_alias")]
        string remoteAlias = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("branch")]
        string branch = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("owner")]
        string owner = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("repo_name")]
        string repoName = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [property: JsonProperty("version")]
        string version = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(UpdateAvailable))]
        [property: JsonProperty("remote_version")]
        string remoteVersion = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("current_hash")]
        string currentHash = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("remote_hash")]
        string remoteHash = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("is_dirty")]
        bool isDirty;

        [ObservableProperty]
        [property: JsonProperty("detached")]
        bool detached;

        [ObservableProperty]
        [property: JsonProperty("commits_behind")]
        List<KlipperUpdateCommitsBehind> commitsBehind = new();

        [ObservableProperty]
        [property: JsonProperty("git_messages")]
        List<object> gitMessages = new();

        [ObservableProperty]
        [property: JsonProperty("full_version_string")]
        string fullVersionString = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("pristine")]
        bool pristine;

        [ObservableProperty]
        [property: JsonProperty("package_count")]
        long packageCount;

        [ObservableProperty]
        [property: JsonProperty("package_list")]
        List<object> packageList = new();

        bool UpdateAvailable => Version != RemoteVersion;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
