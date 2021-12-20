using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperSettingIdleTimeout
    {
        #region Properties
        [JsonProperty("gcode")]
        public string Gcode { get; set; }

        [JsonProperty("timeout")]
        public long Timeout { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
