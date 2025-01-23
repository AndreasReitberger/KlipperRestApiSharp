using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("general")]
        public partial KlipperDatabaseFluiddValueUiSettingsGeneral? General { get; set; }

        [ObservableProperty]

        [JsonProperty("dashboard")]
        public partial KlipperDatabaseFluiddValueUiSettingsDashboard? Dashboard { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
