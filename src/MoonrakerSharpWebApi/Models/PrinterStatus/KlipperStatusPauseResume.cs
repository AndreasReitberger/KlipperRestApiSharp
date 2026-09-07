namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusPauseResume : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("is_paused")]
        public partial bool IsPaused { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusPauseResume);
        #endregion
    }
}
