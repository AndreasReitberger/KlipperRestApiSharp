using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoprintApiPrinterStateSd
    {
        #region Properties
        [JsonProperty("ready", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Ready { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
