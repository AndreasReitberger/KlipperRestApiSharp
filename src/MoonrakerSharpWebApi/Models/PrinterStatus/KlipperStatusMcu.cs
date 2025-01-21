using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMcu : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("mcu_build_versions")]
        public partial string McuBuildVersions { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("mcu_version")]
        public partial string McuVersion { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("last_stats")]
        public partial Dictionary<string, double> LastStats { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("mcu_constants")]
        public partial Dictionary<string, object> McuConstants { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
