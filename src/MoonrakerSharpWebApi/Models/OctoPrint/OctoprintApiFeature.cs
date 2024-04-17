using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiFeature : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("sdSupport")]
        bool sdSupport;

        [ObservableProperty]
        [property: JsonProperty("temperatureGraph")]
        bool temperatureGraph;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
