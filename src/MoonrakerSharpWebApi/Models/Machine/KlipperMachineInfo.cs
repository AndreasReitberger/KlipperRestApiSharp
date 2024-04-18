using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cpu_info")]
        KlipperCpuInfo? cpuInfo;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sd_info")]
        KlipperSdInfo? sdInfo;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("distribution")]
        KlipperDistribution? distribution;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("virtualization")]
        KlipperVirtualization? virtualization;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("available_services")]
        List<string> availableServices = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("service_state")]
        Dictionary<string, KlipperState> serviceState = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("network")]
        Dictionary<string, KlipperNetworkInterface> network = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
