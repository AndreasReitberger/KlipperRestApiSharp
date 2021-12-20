using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperMoonrakerProcessStatsRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperMoonrakerProcessStatsResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
