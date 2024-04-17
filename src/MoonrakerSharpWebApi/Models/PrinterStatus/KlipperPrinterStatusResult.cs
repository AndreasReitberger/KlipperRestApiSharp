using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusResult
    {
        #region Properties
        [JsonProperty("status")]
        public object? Status { get; set; }

        [JsonProperty("eventtime")]
        public double? Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
