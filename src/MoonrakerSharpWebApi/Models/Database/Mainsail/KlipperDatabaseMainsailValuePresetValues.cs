namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValuePresetValues : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("extruder")]
        public partial KlipperDatabaseMainsailHeaterElement? Extruder { get; set; }

        [ObservableProperty]
        [JsonPropertyName("extruder1")]
        public partial KlipperDatabaseMainsailHeaterElement? Extruder1 { get; set; }

        [ObservableProperty]
        [JsonPropertyName("heater_bed")]
        public partial KlipperDatabaseMainsailHeaterElement? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseMainsailValuePresetValues);
        #endregion
    }
}
