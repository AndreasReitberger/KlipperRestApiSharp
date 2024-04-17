using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettingsGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("locale")]
        string locale = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("instanceName")]
        string instanceName = string.Empty;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
