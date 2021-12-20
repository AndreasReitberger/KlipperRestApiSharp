using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.Models
{
    public class KlipperJobListChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public ObservableCollection<KlipperJobQueueItem> NewJobList { get; set; } = new();
        public string NewJobListStatus { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
