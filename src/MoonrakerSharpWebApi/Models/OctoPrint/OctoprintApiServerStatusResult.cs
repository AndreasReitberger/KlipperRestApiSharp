namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiServerStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("server")]
        public partial string Server { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("safemode")]
        public partial object? Safemode { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.OctoprintApiServerStatusResult);
        #endregion
    }
}
