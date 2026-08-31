namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperState : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("active_state")]
        public partial string ActiveState { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("sub_state")]
        public partial string SubState { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
