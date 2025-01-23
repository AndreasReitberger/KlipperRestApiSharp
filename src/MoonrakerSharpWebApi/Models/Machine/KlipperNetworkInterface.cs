using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperNetworkInterface : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("mac_address")]
        public partial string MacAddress { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("ip_addresses")]
        public partial List<KlipperIpAddress> IpAddresses { get; set; } = [];

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
