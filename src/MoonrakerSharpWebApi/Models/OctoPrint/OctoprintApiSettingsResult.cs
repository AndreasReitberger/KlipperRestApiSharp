using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiSettingsResult
    {
        #region Properties
        [JsonProperty("plugins")]
        public Dictionary<string, OctoprintApiPlugin> Plugins { get; set; } = [];

        [JsonProperty("feature")]
        public OctoprintApiFeature? Feature { get; set; }

        [JsonProperty("webcam")]
        public OctoprintApiWebcam? Webcam { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
