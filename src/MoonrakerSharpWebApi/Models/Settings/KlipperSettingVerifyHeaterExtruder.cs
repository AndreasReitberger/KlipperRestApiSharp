using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingVerifyHeaterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("max_error")]
        public partial long MaxError { get; set; }

        [ObservableProperty]

        [JsonProperty("check_gain_time")]
        public partial long CheckGainTime { get; set; }

        [ObservableProperty]

        [JsonProperty("heating_gain")]
        public partial long HeatingGain { get; set; }

        [ObservableProperty]

        [JsonProperty("hysteresis")]
        public partial long Hysteresis { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
