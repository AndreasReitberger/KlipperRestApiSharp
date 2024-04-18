using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketHeaterBedRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heater_bed")]
        KlipperStatusHeaterBed? heaterBed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("toolhead")]
        KlipperStatusToolhead? toolHead;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
