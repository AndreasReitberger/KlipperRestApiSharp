using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? bBed;

        [ObservableProperty]
        [property: JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiPrinterStateTemperatureInfo? chamber;

        [ObservableProperty]
        [property: JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        List<OctoprintApiPrinterStateHistory> history = [];

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
