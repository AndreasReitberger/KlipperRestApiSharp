using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusQueryResult
    {
        // Maybe delete later?
        #region Properties
        [JsonProperty("status")]
        public KlipperPrinterStatus? Status { get; set; }

        [JsonProperty("eventtime")]
        public double Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
