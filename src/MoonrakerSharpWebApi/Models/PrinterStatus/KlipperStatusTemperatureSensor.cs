namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusTemperatureSensor : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("temperature")]
        public partial double? Temperature { get; set; }

        [ObservableProperty]

        [JsonPropertyName("measured_max_temp")]
        public partial double? MeasuredMaxTemperature { get; set; }

        [ObservableProperty]

        [JsonPropertyName("measured_min_temp")]
        public partial double? MeasuredMinTemperature { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
