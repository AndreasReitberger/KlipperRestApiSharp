using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperPrinterStatusSubscriptionRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperPrinterStatusSubscriptionResult Result { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
