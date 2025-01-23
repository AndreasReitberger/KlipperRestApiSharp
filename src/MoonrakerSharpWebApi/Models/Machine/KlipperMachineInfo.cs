using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("cpu_info")]
        public partial KlipperCpuInfo? CpuInfo { get; set; }

        [ObservableProperty]

        [JsonProperty("sd_info")]
        public partial KlipperSdInfo? SdInfo { get; set; }

        [ObservableProperty]

        [JsonProperty("distribution")]
        public partial KlipperDistribution? Distribution { get; set; }

        [ObservableProperty]

        [JsonProperty("virtualization")]
        public partial KlipperVirtualization? Virtualization { get; set; }

        [ObservableProperty]

        [JsonProperty("available_services")]
        public partial List<string> AvailableServices { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("service_state")]
        public partial Dictionary<string, KlipperState> ServiceState { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("network")]
        public partial Dictionary<string, KlipperNetworkInterface> Network { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
