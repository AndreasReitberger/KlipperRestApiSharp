using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueSettingsConfigfiles : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("countPerPage")]
        long countPerPage;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
