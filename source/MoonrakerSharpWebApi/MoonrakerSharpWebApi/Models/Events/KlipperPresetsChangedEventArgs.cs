using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperPresetsChangedEventArgs : KlipperEventArgs
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
