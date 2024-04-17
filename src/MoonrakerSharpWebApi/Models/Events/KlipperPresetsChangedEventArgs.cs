using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperPresetsChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public List<KlipperDatabaseTemperaturePreset> NewPresets { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
