using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerThrottledState : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("rx_bytes")]
        public partial long RxBytes { get; set; }

        [ObservableProperty]

        [JsonProperty("tx_bytes")]
        public partial long TxBytes { get; set; }

        [ObservableProperty]

        [JsonProperty("bandwidth")]
        public partial double Bandwidth { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
