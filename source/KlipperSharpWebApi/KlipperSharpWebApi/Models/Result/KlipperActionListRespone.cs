using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperActionListRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperActionListResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
