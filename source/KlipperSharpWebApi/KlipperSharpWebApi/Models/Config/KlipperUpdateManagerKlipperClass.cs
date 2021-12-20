using Newtonsoft.Json;
using System;

namespace AndreasReitberger.Models
{
    public partial class KlipperUpdateManagerKlipperClass
    {
        #region Properties
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("venv_args")]
        public string VenvArgs { get; set; }

        [JsonProperty("is_system_service")]
        public bool IsSystemService { get; set; }

        [JsonProperty("moved_origin")]
        public Uri MovedOrigin { get; set; }

        [JsonProperty("origin")]
        public Uri Origin { get; set; }

        [JsonProperty("primary_branch")]
        public string PrimaryBranch { get; set; }

        [JsonProperty("enable_node_updates")]
        public bool EnableNodeUpdates { get; set; }

        [JsonProperty("requirements")]
        public string Requirements { get; set; }

        [JsonProperty("install_script")]
        public string InstallScript { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
