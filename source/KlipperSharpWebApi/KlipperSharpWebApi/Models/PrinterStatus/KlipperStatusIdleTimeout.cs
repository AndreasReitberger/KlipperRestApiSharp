using AndreasReitberger.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusIdleTimeout
    {
        #region Properties
        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter), true)]
        //public string State { get; set; }
        public KlipperIdleStates State { get; set; }

        [JsonProperty("printing_time")]
        public double? PrintingTime { get; set; }

        [JsonIgnore]
        public bool ValidState { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
