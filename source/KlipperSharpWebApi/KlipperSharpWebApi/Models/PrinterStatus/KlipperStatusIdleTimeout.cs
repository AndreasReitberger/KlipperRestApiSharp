using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusIdleTimeout
    {
        #region Properties
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("printing_time")]
        public double PrintingTime { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
