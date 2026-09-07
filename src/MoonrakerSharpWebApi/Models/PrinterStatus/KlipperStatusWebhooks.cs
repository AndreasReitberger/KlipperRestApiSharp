namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusWebhooks : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("state")]
        public partial string State { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("state_message")]
        public partial string StateMessage { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperStatusWebhooks);
        #endregion
    }
}
