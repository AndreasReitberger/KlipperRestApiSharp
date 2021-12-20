using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperFile
    {
        #region Properties
        [JsonProperty("filename")]
        public string FileName { get; set; }
        
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("modified")]
        public double Modified { get; set; }

        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        [JsonIgnore]
        public KlipperGcodeMetaResult GcodeMeta { get; set; }

        [JsonIgnore]
        public byte[] Thumbnail { get; set; }

        [JsonIgnore]
        public byte[] Image { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
