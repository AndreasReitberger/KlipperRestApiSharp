using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperServerConfigRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperServerConfigResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
