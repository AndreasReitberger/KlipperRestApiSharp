using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueSettingsConfigfiles : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("countPerPage")]
        public partial long CountPerPage { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
