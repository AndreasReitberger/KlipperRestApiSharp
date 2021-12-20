using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseNamespaceListRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperDatabaseNamespaceListResult Result { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
