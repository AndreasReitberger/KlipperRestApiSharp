using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboard : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("extruder")]
        public KlipperDatabaseMainsailValueDashboardExtruder? extruder;

        [ObservableProperty]
        [property: JsonProperty("control")]
        public KlipperDatabaseMainsailValueDashboardControl? control;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
