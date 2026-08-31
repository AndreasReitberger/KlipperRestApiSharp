namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserActionResult : ObservableObject

    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("username")]
        public partial string Username { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("token")]
        public partial string Token { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("refresh_token")]
        public partial string RefreshToken { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("action")]
        public partial string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
