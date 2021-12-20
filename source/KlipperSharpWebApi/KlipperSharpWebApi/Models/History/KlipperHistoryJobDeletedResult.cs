using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperHistoryJobDeletedResult
    {
        #region Properties
        [JsonProperty("deleted_jobs")]
        public List<string> DeletedJobs { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
