using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEventData
    {
        #region Properties
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public object? Data { get; set; }

        [JsonProperty("event", NullValueHandling = NullValueHandling.Ignore)]
        public string Event { get; set; } = string.Empty;

        [JsonProperty("printer", NullValueHandling = NullValueHandling.Ignore)]
        public string Printer { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
