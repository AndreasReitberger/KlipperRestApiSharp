using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperJobListChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public ObservableCollection<KlipperJobQueueItem> NewJobList { get; set; } = new();
        //public ObservableCollection<IPrint3dJob> NewJobList { get; set; } = new();
        //public string NewJobListStatus { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
