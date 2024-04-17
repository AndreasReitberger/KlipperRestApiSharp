using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperMachineInfoResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("system_info")]
        KlipperMachineInfo? systemInfo;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
