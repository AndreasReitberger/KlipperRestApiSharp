using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboard : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("extruder")]
        public partial KlipperDatabaseMainsailValueDashboardExtruder? Extruder { get; set; }

        [ObservableProperty]
        
        [JsonProperty("control")]
        public partial KlipperDatabaseMainsailValueDashboardControl? Control { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
