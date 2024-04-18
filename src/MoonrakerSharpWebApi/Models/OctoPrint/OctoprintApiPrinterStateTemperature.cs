using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? bBed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? chamber;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        List<OctoprintApiPrinterStateHistory> history = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? tool0;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? tool1;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
