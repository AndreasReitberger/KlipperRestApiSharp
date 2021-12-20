using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperServerTempDataRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperServerTempData Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
