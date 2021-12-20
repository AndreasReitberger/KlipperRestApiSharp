using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperHistoryRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperHistoryResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
