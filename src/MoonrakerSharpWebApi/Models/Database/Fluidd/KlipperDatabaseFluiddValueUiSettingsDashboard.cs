using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettingsDashboard
    {
        #region Properties
        [JsonProperty("tempPresets")]
        public List<KlipperDatabaseFluiddValuePreset> TempPresets { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
