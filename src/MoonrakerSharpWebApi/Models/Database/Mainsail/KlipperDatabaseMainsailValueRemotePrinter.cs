using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueRemotePrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("hostname")]
        public partial string Hostname { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("port")]
        public partial long Port { get; set; }

        [ObservableProperty]

        [JsonProperty("webPort")]
        public partial long WebPort { get; set; }

        [ObservableProperty]

        [JsonProperty("settings")]
        public partial object? Settings { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
