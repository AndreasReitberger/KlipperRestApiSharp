using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiSettingsResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("plugins")]
        public partial Dictionary<string, OctoprintApiPlugin> Plugins { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("feature")]
        public partial OctoprintApiFeature? Feature { get; set; }

        [ObservableProperty]
        
        [JsonProperty("webcam")]
        public partial OctoprintApiWebcam? Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
