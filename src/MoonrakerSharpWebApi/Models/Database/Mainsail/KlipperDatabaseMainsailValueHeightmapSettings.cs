using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueHeightmapSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("mesh")]
        bool mesh;

        [ObservableProperty]
        [property: JsonProperty("scaleVisualMap")]
        bool scaleVisualMap;

        [ObservableProperty]
        [property: JsonProperty("probed")]
        bool probed;

        [ObservableProperty]
        [property: JsonProperty("flat")]
        bool flat;

        [ObservableProperty]
        [property: JsonProperty("wireframe")]
        bool wireframe;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
