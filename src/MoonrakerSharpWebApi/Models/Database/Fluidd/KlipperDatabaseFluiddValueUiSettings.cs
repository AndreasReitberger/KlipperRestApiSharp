using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("general")]
        KlipperDatabaseFluiddValueUiSettingsGeneral? general;

        [ObservableProperty]
        [property: JsonProperty("dashboard")]
        KlipperDatabaseFluiddValueUiSettingsDashboard? dashboard;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
