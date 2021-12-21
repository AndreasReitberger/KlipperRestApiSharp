using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperPrinterStatusRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperPrinterStatusResult Result { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
