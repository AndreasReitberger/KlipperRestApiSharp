using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("cpu_info")]
        KlipperCpuInfo? cpuInfo;

        [ObservableProperty]
        [property: JsonProperty("sd_info")]
        KlipperSdInfo? sdInfo;

        [ObservableProperty]
        [property: JsonProperty("distribution")]
        KlipperDistribution? distribution;

        [ObservableProperty]
        [property: JsonProperty("virtualization")]
        KlipperVirtualization? virtualization;

        [ObservableProperty]
        [property: JsonProperty("available_services")]
        List<string> availableServices = [];

        [ObservableProperty]
        [property: JsonProperty("service_state")]
        Dictionary<string, KlipperState> serviceState = [];

        [ObservableProperty]
        [property: JsonProperty("network")]
        Dictionary<string, KlipperNetworkInterface> network = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
