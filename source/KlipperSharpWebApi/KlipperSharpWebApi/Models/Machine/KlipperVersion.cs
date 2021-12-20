using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperVersion
    {
        #region Properties
        [JsonProperty("major")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long Major { get; set; }

        [JsonProperty("minor")]
        public string Minor { get; set; }

        [JsonProperty("build_number")]
        public string BuildNumber { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
