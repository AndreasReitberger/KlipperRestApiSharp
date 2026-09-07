namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettingsGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("locale")]
        public partial string Locale { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("instanceName")]
        public partial string InstanceName { get; set; } = string.Empty;

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseFluiddValueUiSettingsGeneral);
        #endregion
    }
}
