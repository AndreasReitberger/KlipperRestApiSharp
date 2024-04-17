using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerKlipperClass
    {
        #region Properties
        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;

        [JsonProperty("venv_args")]
        public string VenvArgs { get; set; } = string.Empty;

        [JsonProperty("is_system_service")]
        public bool IsSystemService { get; set; }

        [JsonProperty("moved_origin")]
        public Uri? MovedOrigin { get; set; }

        [JsonProperty("origin")]
        public Uri? Origin { get; set; }

        [JsonProperty("primary_branch")]
        public string PrimaryBranch { get; set; } = string.Empty;

        [JsonProperty("enable_node_updates")]
        public bool EnableNodeUpdates { get; set; }

        [JsonProperty("requirements")]
        public string Requirements { get; set; } = string.Empty;

        [JsonProperty("install_script")]
        public string InstallScript { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
