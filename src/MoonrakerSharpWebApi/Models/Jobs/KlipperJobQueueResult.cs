using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobQueueResult
    {
        #region Properties
        [JsonProperty("queued_jobs")]
        public List<KlipperJobQueueItem> QueuedJobs { get; set; }

        [JsonProperty("queue_state")]
        public string QueueState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
