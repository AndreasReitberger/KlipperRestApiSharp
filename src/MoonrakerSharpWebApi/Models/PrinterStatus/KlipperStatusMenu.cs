using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMenu : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("running")]
        bool running;

        [ObservableProperty]
        [property: JsonProperty("rows")]
        long? rows;

        [ObservableProperty]
        [property: JsonProperty("cols")]
        long? cols;

        [ObservableProperty]
        [property: JsonProperty("timeout")]
        long? timeout;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
