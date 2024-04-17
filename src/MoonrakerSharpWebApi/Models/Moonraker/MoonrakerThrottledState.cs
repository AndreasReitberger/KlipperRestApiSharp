using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerThrottledState
    {
        #region Properties
        [JsonProperty("rx_bytes")]
        public long RxBytes { get; set; }

        [JsonProperty("tx_bytes")]
        public long TxBytes { get; set; }

        [JsonProperty("bandwidth")]
        public double Bandwidth { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
