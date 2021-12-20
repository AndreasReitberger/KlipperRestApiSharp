using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperSettingVerifyHeaterExtruder
    {
        #region Properties
        [JsonProperty("max_error")]
        public long MaxError { get; set; }

        [JsonProperty("check_gain_time")]
        public long CheckGainTime { get; set; }

        [JsonProperty("heating_gain")]
        public long HeatingGain { get; set; }

        [JsonProperty("hysteresis")]
        public long Hysteresis { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
