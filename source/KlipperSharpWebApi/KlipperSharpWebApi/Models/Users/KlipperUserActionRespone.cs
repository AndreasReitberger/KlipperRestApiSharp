using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperUserActionRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperUserActionResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
