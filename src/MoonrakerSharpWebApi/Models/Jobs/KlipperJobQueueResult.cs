using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobQueueResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("queued_jobs")]
        public partial List<IPrint3dJob> QueuedJobs { get; set; } = [];

        [ObservableProperty]
        
        [JsonProperty("queue_state")]
        public partial string QueueState { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion
    }
}
