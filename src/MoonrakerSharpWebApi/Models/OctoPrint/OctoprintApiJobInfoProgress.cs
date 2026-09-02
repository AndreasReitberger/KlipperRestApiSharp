namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobInfoProgress : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("completion")]
        public partial double Completion { get; set; }

        [ObservableProperty]

        [JsonPropertyName("filepos")]
        public partial long Filepos { get; set; }

        [ObservableProperty]

        [JsonPropertyName("printTime")]
        public partial long PrintTime { get; set; }

        [ObservableProperty]

        [JsonPropertyName("printTimeLeft")]
        public partial long PrintTimeLeft { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
