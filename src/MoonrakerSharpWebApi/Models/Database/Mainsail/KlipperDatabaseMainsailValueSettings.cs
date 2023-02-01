using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueSettings
    {
        #region Properties
        [JsonProperty("configfiles")]
        public KlipperDatabaseMainsailValueSettingsConfigfiles Configfiles { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
