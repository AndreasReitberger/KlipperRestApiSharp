using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMenu : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("running")]
        bool running;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rows")]
        long? rows;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cols")]
        long? cols;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("timeout")]
        long? timeout;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
