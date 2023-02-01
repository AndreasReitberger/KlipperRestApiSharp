using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMcu
    {
        #region Properties
        [JsonProperty("mcu_build_versions")]
        public string McuBuildVersions { get; set; }

        [JsonProperty("mcu_version")]
        public string McuVersion { get; set; }

        [JsonProperty("last_stats")]
        public Dictionary<string, double> LastStats { get; set; }

        [JsonProperty("mcu_constants")]
        public Dictionary<string, object> McuConstants { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
