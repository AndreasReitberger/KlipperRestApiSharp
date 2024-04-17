using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseItemResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("namespace")]
        string namespaceValue = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("key")]
        string key = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("value")]
        object? value;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
