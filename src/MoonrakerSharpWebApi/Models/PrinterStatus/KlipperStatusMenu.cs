using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusMenu
    {
        #region Properties
        [JsonProperty("running")]
        public bool Running { get; set; }

        [JsonProperty("rows")]
        public long? Rows { get; set; }

        [JsonProperty("cols")]
        public long? Cols { get; set; }

        [JsonProperty("timeout")]
        public long? Timeout { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
