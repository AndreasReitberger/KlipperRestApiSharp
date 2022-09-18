using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusSystemStats
    {
        #region Properties
        [JsonProperty("sysload")]
        public double? Sysload { get; set; }

        [JsonProperty("memavail")]
        public long? Memavail { get; set; }

        [JsonProperty("cputime")]
        public double? Cputime { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
