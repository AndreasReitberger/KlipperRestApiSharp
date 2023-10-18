using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.Print3dServer.Core.JSON;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperJobQueueResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonProperty("queued_jobs")]
        [property: JsonConverter(typeof(ConcreteTypeConverter<List<KlipperJobQueueItem>>))]
        List<IPrint3dJob> queuedJobs = new();
        //public List<KlipperJobQueueItem> QueuedJobs { get; set; }

        [ObservableProperty]
        [JsonProperty("queue_state")]
        string queueState;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
