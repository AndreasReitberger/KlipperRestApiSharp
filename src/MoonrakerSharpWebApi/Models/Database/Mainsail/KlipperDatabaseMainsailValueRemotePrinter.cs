using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueRemotePrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("hostname")]
        public partial string Hostname { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("port")]
        public partial long Port { get; set; }

        [ObservableProperty]
        [JsonPropertyName("webPort")]
        public partial long WebPort { get; set; }

        [ObservableProperty]
        [JsonPropertyName("settings")]
        public partial object? Settings { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseMainsailValueRemotePrinter);
        #endregion
    }
}
