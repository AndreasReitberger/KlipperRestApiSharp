using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperUserListRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperUserListResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
