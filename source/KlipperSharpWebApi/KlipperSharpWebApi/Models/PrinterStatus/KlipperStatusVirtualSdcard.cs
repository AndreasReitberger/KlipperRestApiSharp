using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusVirtualSdcard
    {
        #region Properties
        [JsonProperty("progress")]
        public double Progress { get; set; }

        [JsonProperty("file_position")]
        public long FilePosition { get; set; }

        [JsonProperty("is_active")]
        public bool IsActive { get; set; }

        [JsonProperty("file_path")]
        public string FilePath { get; set; }

        [JsonProperty("file_size")]
        public long FileSize { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
