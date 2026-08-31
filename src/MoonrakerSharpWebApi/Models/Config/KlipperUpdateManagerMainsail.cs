using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerMainsail : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("type")]
        public partial string Type { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("repo")]
        public partial string Repo { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("path")]
        public partial string Path { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("persistent_files")]
        public partial object? PersistentFiles { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
