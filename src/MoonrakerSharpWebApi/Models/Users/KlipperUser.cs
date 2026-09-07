namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUser : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("username")]
        public partial string Username { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("created_on")]
        public partial double CreatedOn { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperUser);
        #endregion
    }
}
