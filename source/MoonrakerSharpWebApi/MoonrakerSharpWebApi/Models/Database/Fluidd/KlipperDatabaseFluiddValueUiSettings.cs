using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettings
    {
        #region Properties
        [JsonProperty("general")]
        public KlipperDatabaseFluiddValueUiSettingsGeneral General { get; set; }

        [JsonProperty("dashboard")]
        public KlipperDatabaseFluiddValueUiSettingsDashboard Dashboard { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
