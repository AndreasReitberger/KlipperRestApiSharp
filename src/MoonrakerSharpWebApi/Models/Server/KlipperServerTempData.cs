using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerTempData : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("extruder")]
        KlipperExtruderHistory? extruder;

        [ObservableProperty]
        [property: JsonProperty("temperature_fan my_fan")]
        KlipperFanHistory? temperatureFanMyFan;

        [ObservableProperty]
        [property: JsonProperty("temperature_sensor my_sensor")]
        KlipperTemperatureSensorHistory? temperatureSensorMySensor;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
