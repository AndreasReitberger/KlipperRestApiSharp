using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperEndstopQueryRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperEndstopQueryResult Result { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
