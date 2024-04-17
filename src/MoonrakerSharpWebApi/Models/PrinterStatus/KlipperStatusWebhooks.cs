using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusWebhooks
    {
        #region Properties
        [JsonProperty("state")]
        public string State { get; set; } = string.Empty;

        [JsonProperty("state_message")]
        public string StateMessage { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
