using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiFeature : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("sdSupport")]
        public partial bool SdSupport { get; set; }

        [ObservableProperty]
        
        [JsonProperty("temperatureGraph")]
        public partial bool TemperatureGraph { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
