using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperWebSocketError
    {
        #region Properties
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
