using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobQueueResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("queued_jobs")]
        public partial List<IPrint3dJob> QueuedJobs { get; set; } = [];

        [ObservableProperty]

        [JsonPropertyName("queue_state")]
        public partial string QueueState { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperJobQueueResult);

        #endregion
    }
}
