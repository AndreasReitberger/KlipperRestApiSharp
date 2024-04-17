using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVirtualization
    {
        #region Properties
        [JsonProperty("virt_type")]
        public string VirtType { get; set; } = string.Empty;

        [JsonProperty("virt_identifier")]
        public string VirtIdentifier { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
