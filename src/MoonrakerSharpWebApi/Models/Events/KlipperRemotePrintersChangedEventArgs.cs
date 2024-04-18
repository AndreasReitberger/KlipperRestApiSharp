using AndreasReitberger.API.Print3dServer.Core.Events;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperRemotePrintersChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public ObservableCollection<IPrinter3d> NewPrinters { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion
    }
}
