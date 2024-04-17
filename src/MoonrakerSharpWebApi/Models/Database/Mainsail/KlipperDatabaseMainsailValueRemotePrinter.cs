using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueRemotePrinter
    {
        #region Properties
        [JsonProperty("hostname")]
        public string Hostname { get; set; } = string.Empty;

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("webPort")]
        public long WebPort { get; set; }

        [JsonProperty("settings")]
        public object? Settings { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
