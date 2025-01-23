using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettingsGeneral : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("locale")]
        public partial string Locale { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("instanceName")]
        public partial string InstanceName { get; set; } = string.Empty;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
