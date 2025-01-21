using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterStateTemperatureInfo? Bed { get; set; }

        [ObservableProperty]
        
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public partial long? Time { get; set; }

        [ObservableProperty]
        
        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterStateTemperatureInfo? Tool0 { get; set; }

        [ObservableProperty]
        
        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterStateTemperatureInfo? Tool1 { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
