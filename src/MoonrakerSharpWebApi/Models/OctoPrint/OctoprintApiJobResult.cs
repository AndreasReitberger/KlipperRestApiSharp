using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintAbiJobInfoFile? File { get; set; }

        [ObservableProperty]

        [JsonProperty("estimatedPrintTime", NullValueHandling = NullValueHandling.Ignore)]
        public partial long EstimatedPrintTime { get; set; }

        [ObservableProperty]

        [JsonProperty("filament", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiFilamentInfo? Filament { get; set; }

        [ObservableProperty]

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public partial object? User { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
