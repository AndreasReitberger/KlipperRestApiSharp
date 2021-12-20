using Newtonsoft.Json;

namespace AndreasReitberger.Models
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
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
