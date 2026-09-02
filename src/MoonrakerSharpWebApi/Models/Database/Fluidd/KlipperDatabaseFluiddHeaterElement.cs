namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddHeaterElement : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("active")]
        public partial bool Active { get; set; }

        [ObservableProperty]
        [JsonPropertyName("value")]
        public partial long? Value { get; set; }

        [ObservableProperty]
        [JsonPropertyName("type")]
        public partial string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseFluiddHeaterElement);
        #endregion
    }
}
