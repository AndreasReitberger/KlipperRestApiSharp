using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDriver
    {
        #region Properties
        [JsonProperty("cs_actual")]
        public long? CsActual { get; set; }

        [JsonProperty("sg_result")]
        public long? SgResult { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
