namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperIpAddress : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("family")]
        public partial string Family { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("address")]
        public partial string Address { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("is_link_local")]
        public partial bool IsLinkLocal { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
