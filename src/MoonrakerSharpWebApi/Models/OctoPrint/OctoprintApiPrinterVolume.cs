using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterVolume : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("depth")]
        public partial long Depth { get; set; }

        [ObservableProperty]
        
        [JsonProperty("formFactor")]
        public partial string FormFactor { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("height")]
        public partial long Height { get; set; }

        [ObservableProperty]
        
        [JsonProperty("origin")]
        public partial string Origin { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("width")]
        public partial long Width { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
