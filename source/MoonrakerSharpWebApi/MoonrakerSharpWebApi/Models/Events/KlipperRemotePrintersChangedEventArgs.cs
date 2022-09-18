using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperRemotePrintersChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public ObservableCollection<KlipperDatabaseRemotePrinter> NewPrinters { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
