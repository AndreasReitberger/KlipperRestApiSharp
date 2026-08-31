

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDevice : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("device")]
        public partial string Device { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("status")]
        public partial string Status { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("locked_while_printing")]
        public partial bool LockedWhilePrinting { get; set; }

        [ObservableProperty]
        [JsonPropertyName("type")]
        public partial string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
