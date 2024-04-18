using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperNetworkInterface : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mac_address")]
        string macAddress = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ip_addresses")]
        List<KlipperIpAddress> ipAddresses = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rx_bytes")]
        long rxBytes;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tx_bytes")]
        long txBytes;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bandwidth")]
        double bandwidth;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
