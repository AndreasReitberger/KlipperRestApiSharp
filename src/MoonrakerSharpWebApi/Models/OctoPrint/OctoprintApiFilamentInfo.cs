namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiFilamentInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("length")]
        public partial double Length { get; set; }

        [ObservableProperty]

        [JsonPropertyName("volume")]
        public partial double Volume { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
