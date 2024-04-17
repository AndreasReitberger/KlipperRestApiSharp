using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiFeature
    {
        #region Properties
        [JsonProperty("sdSupport")]
        public bool SdSupport { get; set; }

        [JsonProperty("temperatureGraph")]
        public bool TemperatureGraph { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
