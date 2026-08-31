namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusProbe : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("last_z_result")]
        public partial double? LastZResult { get; set; }

        [ObservableProperty]

        [JsonPropertyName("last_query")]
        public partial bool LastQuery { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
