using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEndstopQueryResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("y")]
        string y = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("x")]
        string x = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("z")]
        string z = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
