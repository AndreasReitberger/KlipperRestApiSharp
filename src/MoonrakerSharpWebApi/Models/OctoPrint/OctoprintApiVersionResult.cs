using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiVersionResult
    {
        #region Properties
        [JsonProperty("server")]
        public string Server { get; set; }

        [JsonProperty("api")]
        public string Api { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
