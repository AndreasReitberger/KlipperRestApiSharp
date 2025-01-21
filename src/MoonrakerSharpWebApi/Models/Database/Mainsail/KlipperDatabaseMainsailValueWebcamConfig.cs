using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("icon")]
        public partial string Icon { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("service")]
        public partial string Service { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("targetFps")]
        public partial long TargetFps { get; set; }

        [ObservableProperty]
        
        [JsonProperty("url")]
        public partial string Url { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("flipX")]
        public partial bool FlipX { get; set; }

        [ObservableProperty]
        
        [JsonProperty("flipY")]
        public partial bool FlipY { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
