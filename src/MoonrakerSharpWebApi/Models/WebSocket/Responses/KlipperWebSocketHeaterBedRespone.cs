using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models.WebSocket
{
    public partial class KlipperWebSocketHeaterBedRespone : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("heater_bed", Required = Required.Always)]
        KlipperStatusHeaterBed? heaterBed;

        [ObservableProperty]
        [property: JsonProperty("toolhead", Required = Required.Always)]
        KlipperStatusToolhead? toolHead;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
