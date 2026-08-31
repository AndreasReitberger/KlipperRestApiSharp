using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("cpu_info")]
        public partial KlipperCpuInfo? CpuInfo { get; set; }

        [ObservableProperty]

        [JsonPropertyName("sd_info")]
        public partial KlipperSdInfo? SdInfo { get; set; }

        [ObservableProperty]

        [JsonPropertyName("distribution")]
        public partial KlipperDistribution? Distribution { get; set; }

        [ObservableProperty]

        [JsonPropertyName("virtualization")]
        public partial KlipperVirtualization? Virtualization { get; set; }

        [ObservableProperty]

        [JsonPropertyName("available_services")]
        public partial List<string> AvailableServices { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("service_state")]
        public partial Dictionary<string, KlipperState> ServiceState { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("network")]
        public partial Dictionary<string, KlipperNetworkInterface> Network { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
