namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMenu : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("running")]
        public partial bool Running { get; set; }

        [ObservableProperty]

        [JsonPropertyName("rows")]
        public partial long? Rows { get; set; }

        [ObservableProperty]

        [JsonPropertyName("cols")]
        public partial long? Cols { get; set; }

        [ObservableProperty]

        [JsonPropertyName("timeout")]
        public partial long? Timeout { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
