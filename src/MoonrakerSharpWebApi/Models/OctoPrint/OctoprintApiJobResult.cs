namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("file")]
        public partial OctoprintAbiJobInfoFile? File { get; set; }

        [ObservableProperty]

        [JsonPropertyName("estimatedPrintTime")]
        public partial long EstimatedPrintTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("filament")]
        public partial OctoprintApiFilamentInfo? Filament { get; set; }

        [ObservableProperty]

        [JsonPropertyName("user")]
        public partial object? User { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
