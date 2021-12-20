using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperHeaterElement
    {
        #region Properties
        [JsonProperty("bool")]
        public bool Bool { get; set; }

        [JsonProperty("value")]
        public long? Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
