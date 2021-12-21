using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperPrinterStatusResult
    {
        #region Properties
        [JsonProperty("status")]
        public object Status { get; set; }

        [JsonProperty("eventtime")]
        public double Eventtime { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
