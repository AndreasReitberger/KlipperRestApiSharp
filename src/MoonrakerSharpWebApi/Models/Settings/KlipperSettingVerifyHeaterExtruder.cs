using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingVerifyHeaterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_error")]
        long maxError;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("check_gain_time")]
        long checkGainTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heating_gain")]
        long heatingGain;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hysteresis")]
        long hysteresis;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
