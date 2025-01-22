using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketMcuRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("mcu")]
        public partial KlipperStatusMcu? Mcu { get; set; }

        [ObservableProperty]
        
        [JsonProperty("system_stats")]
        public partial KlipperStatusSystemStats? SystemStats { get; set; }

        [ObservableProperty]
        
        [JsonProperty("toolhead")]
        public partial KlipperStatusToolhead? Toolhead { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
