namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusSystemStats : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("sysload")]
        public partial double? Sysload { get; set; }

        [ObservableProperty]

        [JsonPropertyName("memavail")]
        public partial long? Memavail { get; set; }

        [ObservableProperty]

        [JsonPropertyName("cputime")]
        public partial double? Cputime { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
