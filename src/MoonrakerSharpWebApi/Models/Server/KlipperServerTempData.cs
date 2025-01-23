using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerTempData : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("extruder")]
        public partial KlipperExtruderHistory? Extruder { get; set; }

        [ObservableProperty]

        [JsonProperty("temperature_fan my_fan")]
        public partial KlipperFanHistory? TemperatureFanMyFan { get; set; }

        [ObservableProperty]

        [JsonProperty("temperature_sensor my_sensor")]
        public partial KlipperTemperatureSensorHistory? TemperatureSensorMySensor { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
