using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerTempData
    {
        #region Properties
        [JsonProperty("extruder")]
        public KlipperExtruderHistory? Extruder { get; set; }

        [JsonProperty("temperature_fan my_fan")]
        public KlipperFanHistory? TemperatureFanMyFan { get; set; }

        [JsonProperty("temperature_sensor my_sensor")]
        public KlipperTemperatureSensorHistory? TemperatureSensorMySensor { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
