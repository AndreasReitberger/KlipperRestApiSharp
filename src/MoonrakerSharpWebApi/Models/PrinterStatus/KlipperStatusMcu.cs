using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMcu : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mcu_build_versions")]
        string mcuBuildVersions = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mcu_version")]
        string mcuVersion = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("last_stats")]
        Dictionary<string, double> lastStats = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mcu_constants")]
        Dictionary<string, object> mcuConstants = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
