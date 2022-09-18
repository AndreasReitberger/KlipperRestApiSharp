using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusWebhooks
    {
        #region Properties
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("state_message")]
        public string StateMessage { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
