using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingVerifyHeaterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("max_error")]
        long maxError;

        [ObservableProperty]
        [property: JsonProperty("check_gain_time")]
        long checkGainTime;

        [ObservableProperty]
        [property: JsonProperty("heating_gain")]
        long heatingGain;

        [ObservableProperty]
        [property: JsonProperty("hysteresis")]
        long hysteresis;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
