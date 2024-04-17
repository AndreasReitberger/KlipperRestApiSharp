using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboardControl : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("useCross")]
        bool useCross;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
