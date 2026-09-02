namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDistribution : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("name")]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("id")]
        public partial string Id { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("version")]
        public partial long Version { get; set; }

        [ObservableProperty]

        [JsonPropertyName("version_parts")]
        public partial KlipperVersion? KlipperVersion { get; set; }

        [ObservableProperty]

        [JsonPropertyName("like")]
        public partial string Like { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("codename")]
        public partial string Codename { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDistribution);
        #endregion
    }
}
