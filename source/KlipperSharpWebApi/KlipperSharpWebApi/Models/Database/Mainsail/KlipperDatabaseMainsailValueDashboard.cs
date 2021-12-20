using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseMainsailValueDashboard
    {
        #region Properties
        [JsonProperty("extruder")]
        public KlipperDatabaseMainsailValueDashboardExtruder Extruder { get; set; }

        [JsonProperty("control")]
        public KlipperDatabaseMainsailValueDashboardControl Control { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
