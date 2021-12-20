using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperDatabaseMainsailValueDashboardExtruder
    {
        #region Properties
        [JsonProperty("feedamount")]
        public long Feedamount { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
