using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperPrinterStateMessageRespone
    {
        #region Properties
        [JsonProperty("result")]
        public KlipperPrinterStateMessageResult Result { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
