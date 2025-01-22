using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperHistoryJobDeletedResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("deleted_jobs")]
        public partial List<string> DeletedJobs { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
