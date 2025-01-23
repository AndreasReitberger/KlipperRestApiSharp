using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfoResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("system_info")]
        public partial KlipperMachineInfo? SystemInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
