using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperature? temperature;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sd", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateSd? sd;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterState? state;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
