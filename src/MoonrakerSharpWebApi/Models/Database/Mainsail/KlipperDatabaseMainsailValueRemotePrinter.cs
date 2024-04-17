using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueRemotePrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("hostname")]
        public string hostname = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("port")]
        public long port;

        [ObservableProperty]
        [property: JsonProperty("webPort")]
        public long webPort;

        [ObservableProperty]
        [property: JsonProperty("settings")]
        public object? settings;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
