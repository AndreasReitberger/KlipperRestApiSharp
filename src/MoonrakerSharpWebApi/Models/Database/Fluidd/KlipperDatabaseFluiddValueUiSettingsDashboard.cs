using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettingsDashboard : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("tempPresets")]
        public partial List<KlipperDatabaseFluiddValuePreset> TempPresets { get; set; } = [];

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
