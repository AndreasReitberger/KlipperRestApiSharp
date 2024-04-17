using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperWebCamConfigChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public List<KlipperDatabaseWebcamConfig> NewConfig { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
