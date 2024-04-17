using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperJobListChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public ObservableCollection<KlipperJobQueueItem> NewJobList { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
