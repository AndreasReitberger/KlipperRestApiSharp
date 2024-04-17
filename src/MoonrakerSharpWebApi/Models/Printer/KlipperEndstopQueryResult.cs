using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEndstopQueryResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("y")]
        string y = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("x")]
        string x = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("z")]
        string z = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
