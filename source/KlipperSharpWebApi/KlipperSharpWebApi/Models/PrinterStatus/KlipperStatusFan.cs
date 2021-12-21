using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusFan
    {
        #region Properties
        [JsonProperty("speed")]
        public double? Speed { get; set; }

        [JsonProperty("rpm")]
        public double? Rpm { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
