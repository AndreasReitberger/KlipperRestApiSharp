using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDevice
    {
        #region Properties
        [JsonProperty("device")]
        public string Device { get; set; } = string.Empty;

        [JsonProperty("status")]
        public string Status { get; set; } = string.Empty;

        [JsonProperty("locked_while_printing")]
        public bool LockedWhilePrinting { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
