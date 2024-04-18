using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseItemResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("namespace")]
        string namespaceValue = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("key")]
        string key = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("value")]
        object? value;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
