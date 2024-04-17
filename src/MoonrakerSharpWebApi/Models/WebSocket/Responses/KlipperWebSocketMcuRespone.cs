using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketMcuRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("mcu", Required = Required.Always)]
        KlipperStatusMcu? mcu;

        [ObservableProperty]
        [property: JsonProperty("system_stats", Required = Required.Always)]
        KlipperStatusSystemStats? systemStats;

        [ObservableProperty]
        [property: JsonProperty("toolhead", Required = Required.Always)]
        KlipperStatusToolhead? toolhead;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
