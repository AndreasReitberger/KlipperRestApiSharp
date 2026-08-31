using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("configfiles")]
        public partial KlipperDatabaseMainsailValueSettingsConfigfiles? Configfiles { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
