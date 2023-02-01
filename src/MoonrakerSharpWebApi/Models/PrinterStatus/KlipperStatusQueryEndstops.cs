using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperStatusQueryEndstops
    {
        #region Properties
        [JsonProperty("last_query")]
        public KlipperEndstopQueryResult LastQuery { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
