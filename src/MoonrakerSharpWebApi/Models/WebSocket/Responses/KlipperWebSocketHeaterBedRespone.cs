using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketHeaterBedRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("heater_bed")]
        public partial KlipperStatusHeaterBed? HeaterBed { get; set; }

        [ObservableProperty]

        [JsonPropertyName("toolhead")]
        public partial KlipperStatusToolhead? ToolHead { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
