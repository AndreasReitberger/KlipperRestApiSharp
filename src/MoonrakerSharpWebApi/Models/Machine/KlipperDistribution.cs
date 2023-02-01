using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDistribution
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        //[JsonConverter(typeof(ParseStringConverter))]
        public long Version { get; set; }

        [JsonProperty("version_parts")]
        public KlipperVersion KlipperVersion { get; set; }

        [JsonProperty("like")]
        public string Like { get; set; }

        [JsonProperty("codename")]
        public string Codename { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
