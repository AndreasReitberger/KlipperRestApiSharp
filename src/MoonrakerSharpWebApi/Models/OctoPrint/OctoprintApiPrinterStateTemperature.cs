using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateTemperature : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterStateTemperatureInfo? BBed { get; set; }

        [ObservableProperty]

        [JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        public partial OctoprintApiPrinterStateTemperatureInfo? Chamber { get; set; }

        [ObservableProperty]

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public partial List<OctoprintApiPrinterStateHistory> History { get; set; } = [];

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
