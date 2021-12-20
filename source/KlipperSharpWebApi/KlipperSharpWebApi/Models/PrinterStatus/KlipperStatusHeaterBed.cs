using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusHeaterBed
    {
        #region Properties
        [JsonProperty("temperature")]
        public double Temperature { get; set; }

        [JsonProperty("target")]
        public double Target { get; set; }

        [JsonProperty("power")]
        public double Power { get; set; }

        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
