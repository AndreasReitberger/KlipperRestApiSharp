using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddHeaterElement : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("active")]
        bool active;

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
