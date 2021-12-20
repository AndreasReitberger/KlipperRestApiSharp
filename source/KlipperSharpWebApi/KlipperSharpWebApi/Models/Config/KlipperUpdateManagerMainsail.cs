using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperUpdateManagerMainsail
    {
        #region Properties
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("repo")]
        public string Repo { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("persistent_files")]
        public object PersistentFiles { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
