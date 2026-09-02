namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailHeaterElement : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("bool")]
        public partial bool BoolValue { get; set; }

        [ObservableProperty]
        [JsonPropertyName("value")]
        public partial long? Value { get; set; }

        [ObservableProperty]
        [JsonPropertyName("type")]
        public partial string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseMainsailHeaterElement);
        #endregion
    }
}
