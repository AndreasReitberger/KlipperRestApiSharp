namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingForceMove : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("enable_force_move")]
        public partial bool EnableForceMove { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperSettingForceMove);
        #endregion
    }
}
