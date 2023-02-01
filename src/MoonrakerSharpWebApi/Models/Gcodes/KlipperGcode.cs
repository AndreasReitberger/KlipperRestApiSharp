using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcode
    {
        #region Properties
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("time")]
        public double Time { get; set; }

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
