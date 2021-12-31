using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusDriverRespone
    {
        #region Properties
        [JsonProperty("drv_status")]
        public KlipperStatusDriver DrvStatus { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
