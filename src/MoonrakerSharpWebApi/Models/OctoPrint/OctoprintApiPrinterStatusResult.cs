using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterStateTemperature? Temperature { get; set; }

        [ObservableProperty]

        [JsonProperty("sd", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterStateSd? Sd { get; set; }

        [ObservableProperty]

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterState? State { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
