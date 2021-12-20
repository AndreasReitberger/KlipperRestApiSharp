using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperHistoryTotalRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperHistoryTotalResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
