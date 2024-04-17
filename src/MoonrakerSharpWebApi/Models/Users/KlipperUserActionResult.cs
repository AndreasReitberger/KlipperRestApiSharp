using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserActionResult

    {
        #region Properties
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonProperty("action")]
        public string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
