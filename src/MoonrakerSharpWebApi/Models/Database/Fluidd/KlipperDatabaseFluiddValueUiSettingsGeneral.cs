using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueUiSettingsGeneral
    {
        #region Properties
        [JsonProperty("locale")]
        public string Locale { get; set; } = string.Empty;

        [JsonProperty("instanceName")]
        public string InstanceName { get; set; } = string.Empty;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
