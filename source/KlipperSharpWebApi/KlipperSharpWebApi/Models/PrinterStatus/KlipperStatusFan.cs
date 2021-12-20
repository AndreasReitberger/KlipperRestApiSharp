using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusFan
    {
        #region Properties
        [JsonProperty("speed")]
        public long? Speed { get; set; }

        [JsonProperty("rpm")]
        public long? Rpm { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
