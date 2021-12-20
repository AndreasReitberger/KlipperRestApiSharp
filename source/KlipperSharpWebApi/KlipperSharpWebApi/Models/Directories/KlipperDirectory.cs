using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDirectory
    {
        #region Properties
        [JsonProperty("dirname")]
        public string DirectoryName { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("modified")]
        public double Modified { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
