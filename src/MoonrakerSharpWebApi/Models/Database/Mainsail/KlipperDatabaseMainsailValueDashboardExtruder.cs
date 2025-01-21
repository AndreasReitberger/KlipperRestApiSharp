using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueDashboardExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("feedamount")]
        public partial long Feedamount { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
