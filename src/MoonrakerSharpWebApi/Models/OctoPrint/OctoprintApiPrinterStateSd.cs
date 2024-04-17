using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterStateSd
    {
        #region Properties
        [JsonProperty("ready", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ready { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
