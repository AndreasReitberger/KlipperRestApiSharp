using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailHeaterElement : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bool")]
        bool boolValue;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("value")]
        long? value;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("type")]
        string type = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
