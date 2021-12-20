using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperUpdateStatusRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperUpdateStatusResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

}
