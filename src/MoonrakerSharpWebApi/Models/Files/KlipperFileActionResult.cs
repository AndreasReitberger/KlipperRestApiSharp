namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("item")]
        public partial KlipperFileItem? Item { get; set; }

        [ObservableProperty]

        [JsonPropertyName("print_started")]
        public partial bool PrintStarted { get; set; }

        [ObservableProperty]

        [JsonPropertyName("print_queued")]
        public partial bool PrintQueued { get; set; }

        [ObservableProperty]

        [JsonPropertyName("action")]
        public partial string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
