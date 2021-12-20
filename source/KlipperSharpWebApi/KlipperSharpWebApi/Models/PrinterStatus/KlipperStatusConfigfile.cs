using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusConfigfile
    {
        #region Properties
        [JsonProperty("warnings")]
        public List<object> Warnings { get; set; } = new();

        [JsonProperty("config")]
        public KlipperConfig Config { get; set; }

        [JsonProperty("settings")]
        public KlipperSettings Settings { get; set; }

        [JsonProperty("save_config_pending")]
        public bool SaveConfigPending { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
