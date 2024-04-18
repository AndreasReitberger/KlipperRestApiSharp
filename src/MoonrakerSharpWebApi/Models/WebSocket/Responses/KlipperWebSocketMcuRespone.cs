using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketMcuRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mcu")]
        KlipperStatusMcu? mcu;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("system_stats")]
        KlipperStatusSystemStats? systemStats;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("toolhead")]
        KlipperStatusToolhead? toolhead;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
