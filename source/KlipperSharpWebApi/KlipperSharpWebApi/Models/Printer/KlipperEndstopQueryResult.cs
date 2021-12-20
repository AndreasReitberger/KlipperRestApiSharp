using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperEndstopQueryResult
    {
        #region Properties
        [JsonProperty("y")]
        public string Y { get; set; }

        [JsonProperty("x")]
        public string X { get; set; }

        [JsonProperty("z")]
        public string Z { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
