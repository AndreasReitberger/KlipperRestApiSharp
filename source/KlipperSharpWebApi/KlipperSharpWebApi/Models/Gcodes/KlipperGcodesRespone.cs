using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperGcodesRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperGcodesResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
