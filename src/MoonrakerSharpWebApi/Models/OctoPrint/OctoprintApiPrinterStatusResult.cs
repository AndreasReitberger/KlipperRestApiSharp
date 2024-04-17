using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStatusResult
    {
        #region Properties
        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiPrinterStateTemperature? Temperature { get; set; }

        [JsonProperty("sd", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiPrinterStateSd? Sd { get; set; }

        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public OctoprintApiPrinterState? State { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
