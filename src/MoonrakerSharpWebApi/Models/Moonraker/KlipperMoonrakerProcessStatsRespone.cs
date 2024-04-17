using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMoonrakerProcessStatsRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperMoonrakerProcessStatsResult? Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
