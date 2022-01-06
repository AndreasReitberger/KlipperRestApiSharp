using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperPrinterStatusSubscriptionResult
    {
        #region Properties
        [JsonProperty("status")]
        public KlipperPrinterStatusSubscriptionStatus Status { get; set; }

        [JsonProperty("eventtime")]
        public double? Eventtime { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
