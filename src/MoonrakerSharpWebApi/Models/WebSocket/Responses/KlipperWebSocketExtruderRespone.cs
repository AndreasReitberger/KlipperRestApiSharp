using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketExtruderRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("extruder")]
        public partial KlipperStatusExtruder? Extruder { get; set; }

        [ObservableProperty]
        [JsonPropertyName("toolhead")]
        public partial KlipperStatusToolhead? ToolHead { get; set; }

        [ObservableProperty]
        [JsonPropertyName("extruder1")]
        public partial KlipperStatusExtruder? Extruder1 { get; set; }

        [ObservableProperty]
        [JsonPropertyName("extruder2")]
        public partial KlipperStatusExtruder? Extruder2 { get; set; }

        [ObservableProperty]
        [JsonPropertyName("extruder3")]
        public partial KlipperStatusExtruder? Extruder3 { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
