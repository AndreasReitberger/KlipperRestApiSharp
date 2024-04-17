using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobResult
    {
        #region Properties
        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintAbiJobInfoFile? File { get; set; }

        [JsonProperty("estimatedPrintTime", NullValueHandling = NullValueHandling.Ignore)]
        public long EstimatedPrintTime { get; set; }

        [JsonProperty("filament", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiFilamentInfo? Filament { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public object? User { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
