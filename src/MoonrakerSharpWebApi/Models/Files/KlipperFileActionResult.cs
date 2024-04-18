using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("item")]
        public KlipperFileItem? item;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("print_started")]
        public bool printStarted;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("print_queued")]
        public bool printQueued;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("action")]
        public string action = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
