using Newtonsoft.Json;

namespace AndreasReitberger.Models.WebSocket
{
    public partial class KlipperWebSocketHeaterBedRespone
    {
        #region Properties
        [JsonProperty("heater_bed", Required = Required.Always)]
        public KlipperStatusHeaterBed HeaterBed { get; set; }

        [JsonProperty("toolhead", Required = Required.Always)]
        public KlipperStatusToolhead ToolHead { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
