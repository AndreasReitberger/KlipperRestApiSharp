using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboardControl
    {
        #region Properties
        [JsonProperty("useCross")]
        public bool UseCross { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
