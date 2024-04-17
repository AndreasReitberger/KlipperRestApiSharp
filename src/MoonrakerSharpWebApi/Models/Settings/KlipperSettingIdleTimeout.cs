using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettingIdleTimeout : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("gcode")]
        string gcode = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("timeout")]
        long timeout;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
