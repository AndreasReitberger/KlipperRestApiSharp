using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueRemotePrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hostname")]
        public string hostname = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("port")]
        public long port;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("webPort")]
        public long webPort;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("settings")]
        public object? settings;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
