namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateCommitsBehind : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("sha")]
        public partial string Sha { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("author")]
        public partial string Author { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("date")]
        public partial long Date { get; set; }

        [ObservableProperty]

        [JsonPropertyName("subject")]
        public partial string Subject { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("message")]
        public partial string Message { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("tag")]
        public partial object? Tag { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
