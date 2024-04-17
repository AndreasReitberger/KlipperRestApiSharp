using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfoResult
    {
        #region Properties
        [JsonProperty("system_info")]
        public KlipperMachineInfo? SystemInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
