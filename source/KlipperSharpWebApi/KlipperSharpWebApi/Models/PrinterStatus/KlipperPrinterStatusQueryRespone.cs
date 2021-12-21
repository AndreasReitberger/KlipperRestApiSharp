using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    // Maybe delete later?
    public partial class KlipperPrinterStatusQueryRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperPrinterStatusQueryResult Result { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
