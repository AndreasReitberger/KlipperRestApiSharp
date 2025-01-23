using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMenu : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("running")]
        public partial bool Running { get; set; }

        [ObservableProperty]

        [JsonProperty("rows")]
        public partial long? Rows { get; set; }

        [ObservableProperty]

        [JsonProperty("cols")]
        public partial long? Cols { get; set; }

        [ObservableProperty]

        [JsonProperty("timeout")]
        public partial long? Timeout { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
