using System.Collections.Generic;

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
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperDatabaseFluiddValueUiSettingsDashboard);
        #endregion
    }
}
