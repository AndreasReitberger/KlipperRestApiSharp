using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateTemperature
    {
        #region Properties
        [JsonProperty("bed", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiPrinterStateTemperatureInfo? Bed { get; set; }

        [JsonProperty("chamber", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiPrinterStateTemperatureInfo? Chamber { get; set; }

        [JsonProperty("history", NullValueHandling = NullValueHandling.Ignore)]
        public List<OctoprintApiPrinterStateHistory> History { get; set; } = [];

        [JsonProperty("tool0", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiPrinterStateTemperatureInfo? Tool0 { get; set; }

        [JsonProperty("tool1", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiPrinterStateTemperatureInfo? Tool1 { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
