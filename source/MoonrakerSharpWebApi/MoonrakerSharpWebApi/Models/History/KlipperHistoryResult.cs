using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryResult
    {
        #region Properties
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("jobs")]
        public List<KlipperJobItem> Jobs { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
