namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerTempData : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("extruder")]
        public partial KlipperExtruderHistory? Extruder { get; set; }

        [ObservableProperty]

        [JsonPropertyName("temperature_fan my_fan")]
        public partial KlipperFanHistory? TemperatureFanMyFan { get; set; }

        [ObservableProperty]

        [JsonPropertyName("temperature_sensor my_sensor")]
        public partial KlipperTemperatureSensorHistory? TemperatureSensorMySensor { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperServerTempData);
        #endregion
    }
}
