using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiVersionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("server")]
        string server = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("api")]
        string api = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("text")]
        string text = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
