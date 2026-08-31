using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperNetworkInterface : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("mac_address")]
        public partial string MacAddress { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("ip_addresses")]
        public partial List<KlipperIpAddress> IpAddresses { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("rx_bytes")]
        public partial long RxBytes { get; set; }

        [ObservableProperty]

        [JsonPropertyName("tx_bytes")]
        public partial long TxBytes { get; set; }

        [ObservableProperty]

        [JsonPropertyName("bandwidth")]
        public partial double Bandwidth { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
