using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUser
    {
        #region Properties
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("created_on")]
        public double CreatedOn { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
