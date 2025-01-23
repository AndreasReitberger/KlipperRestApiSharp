using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketHeaterBedRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("heater_bed")]
        public partial KlipperStatusHeaterBed? HeaterBed { get; set; }

        [ObservableProperty]

        [JsonProperty("toolhead")]
        public partial KlipperStatusToolhead? ToolHead { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
