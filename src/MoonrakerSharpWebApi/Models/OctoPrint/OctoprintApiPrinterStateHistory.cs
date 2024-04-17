using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateHistory : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? bed;

        [ObservableProperty]
        [property: JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        long? time;

        [ObservableProperty]
        [property: JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? tool0;

        [ObservableProperty]
        [property: JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? tool1;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
