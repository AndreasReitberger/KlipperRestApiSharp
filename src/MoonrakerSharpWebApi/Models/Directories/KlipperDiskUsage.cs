using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDiskUsage
    {
        #region Properties
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("used")]
        public long Used { get; set; }

        [JsonProperty("free")]
        public long Free { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
