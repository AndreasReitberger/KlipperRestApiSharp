using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusTemperatureSensor : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("temperature")]
        double? temperature;

        [ObservableProperty]
        [property: JsonProperty("measured_max_temp")]
        double? measuredMaxTemperature;

        [ObservableProperty]
        [property: JsonProperty("measured_min_temp")]
        double? measuredMinTemperature;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
