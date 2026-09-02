namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingPauseResume : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("recover_velocity")]
        public partial long RecoverVelocity { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
