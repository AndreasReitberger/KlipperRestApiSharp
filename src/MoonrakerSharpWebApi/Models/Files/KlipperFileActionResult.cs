using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileActionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("item")]
        public partial KlipperFileItem? Item { get; set; }

        [ObservableProperty]

        [JsonProperty("print_started")]
        public partial bool PrintStarted { get; set; }

        [ObservableProperty]

        [JsonProperty("print_queued")]
        public partial bool PrintQueued { get; set; }

        [ObservableProperty]

        [JsonProperty("action")]
        public partial string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
