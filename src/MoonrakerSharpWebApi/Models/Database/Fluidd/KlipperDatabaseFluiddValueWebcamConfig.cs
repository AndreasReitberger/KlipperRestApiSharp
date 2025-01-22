using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("id")]
        public partial Guid Id { get; set; }

        [ObservableProperty]
        
        [JsonProperty("enabled")]
        public partial bool Enabled { get; set; }

        [ObservableProperty]
        
        [JsonProperty("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("service")]
        public partial string Service { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("targetFps")]
        public partial long Fpstarget { get; set; }

        [ObservableProperty]
        
        [JsonProperty("urlStream")]
        public partial string UrlStream { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("urlSnapshot")]
        public partial string UrlSnapshot { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("flipX")]
        public partial bool FlipX { get; set; }

        [ObservableProperty]
        
        [JsonProperty("flipY")]
        public partial bool FlipY { get; set; }

        [ObservableProperty]
        
        [JsonProperty("rotation")]
        public partial int? Rotation { get; set; } = 0;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
