using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperGcodeMetaRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperGcodeMetaResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
