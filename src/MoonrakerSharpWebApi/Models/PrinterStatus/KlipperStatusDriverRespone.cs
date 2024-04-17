using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusDriverRespone
    {
        #region Properties
        [JsonProperty("drv_status")]
        public KlipperStatusDriver? DrvStatus { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
