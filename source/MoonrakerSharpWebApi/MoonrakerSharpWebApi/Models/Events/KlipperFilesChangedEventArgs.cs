using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperFilesChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public ObservableCollection<KlipperFile> NewFiles { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
