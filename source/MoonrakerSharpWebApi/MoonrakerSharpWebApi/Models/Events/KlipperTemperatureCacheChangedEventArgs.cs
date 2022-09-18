using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperTemperatureCacheChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public Dictionary<string, KlipperTemperatureSensorHistory> CachedTemperatures { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
