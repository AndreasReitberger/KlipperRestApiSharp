using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseItemResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("namespace")]
        public partial string NamespaceValue { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("key")]
        public partial string Key { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("value")]
        public partial object? Value { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
