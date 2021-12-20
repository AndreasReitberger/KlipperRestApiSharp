using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperServerConfigResult
    {
        #region Properties
        [JsonProperty("config")]
        public KlipperServerConfig Config { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
