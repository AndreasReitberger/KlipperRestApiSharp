using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueHeightmapSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mesh")]
        bool mesh;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("scaleVisualMap")]
        bool scaleVisualMap;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("probed")]
        bool probed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flat")]
        bool flat;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("wireframe")]
        bool wireframe;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
