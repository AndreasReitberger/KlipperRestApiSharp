using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperState
    {
        #region Properties
        [JsonProperty("active_state")]
        public string ActiveState { get; set; } = string.Empty;

        [JsonProperty("sub_state")]
        public string SubState { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
