using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperRootInfo
    {
        #region Properties
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("permissions")]
        public string Permissions { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
