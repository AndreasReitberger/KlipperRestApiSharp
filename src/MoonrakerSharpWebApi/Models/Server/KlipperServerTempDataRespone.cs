using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServerTempDataRespone
    {
        #region Properties
        [JsonProperty("result")]
        public Dictionary<string, KlipperTemperatureSensorHistory> Result { get; set; } = new();
        //public KlipperServerTempData Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
