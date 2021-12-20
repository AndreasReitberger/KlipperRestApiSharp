using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperMachineInfo
    {
        #region Properties
        [JsonProperty("cpu_info")]
        public KlipperCpuInfo CpuInfo { get; set; }

        [JsonProperty("sd_info")]
        public KlipperSdInfo SdInfo { get; set; }

        [JsonProperty("distribution")]
        public KlipperDistribution Distribution { get; set; }

        [JsonProperty("virtualization")]
        public KlipperVirtualization Virtualization { get; set; }

        [JsonProperty("available_services")]
        public List<string> AvailableServices { get; set; }

        [JsonProperty("service_state")]
        public Dictionary<string, KlipperState> ServiceState { get; set; }

        [JsonProperty("network")]
        public Dictionary<string, KlipperNetworkInterface> Network { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
