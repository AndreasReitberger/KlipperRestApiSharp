using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseItemResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("namespace")]
        public partial string NamespaceValue { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("key")]
        public partial string Key { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("value")]
        public partial object? Value { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
