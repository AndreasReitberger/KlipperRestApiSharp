namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("job")]
        public partial OctoprintApiJobResult? Job { get; set; }

        [ObservableProperty]

        [JsonPropertyName("progress")]
        public partial OctoprintApiJobInfoProgress? Progress { get; set; }

        [ObservableProperty]

        [JsonPropertyName("state")]
        public partial string State { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
