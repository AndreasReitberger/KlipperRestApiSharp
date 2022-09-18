using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperNetworkInterface
    {
        #region Properties
        [JsonProperty("mac_address")]
        public string MacAddress { get; set; }

        [JsonProperty("ip_addresses")]
        public List<KlipperIpAddress> IpAddresses { get; set; }

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
