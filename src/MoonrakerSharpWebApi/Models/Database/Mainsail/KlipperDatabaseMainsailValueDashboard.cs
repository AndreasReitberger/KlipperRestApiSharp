using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboard
    {
        #region Properties
        [JsonProperty("extruder")]
        public KlipperDatabaseMainsailValueDashboardExtruder? Extruder { get; set; }

        [JsonProperty("control")]
        public KlipperDatabaseMainsailValueDashboardControl? Control { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
