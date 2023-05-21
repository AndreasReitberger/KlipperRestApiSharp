using Newtonsoft.Json;
#if ConcurrentDictionary
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperTemperatureCacheChangedEventArgs : KlipperEventArgs
    {
        #region Properties
#if ConcurrentDictionary
        public ConcurrentDictionary<string, KlipperTemperatureSensorHistory> CachedTemperatures { get; set; } = new();
#else
        public Dictionary<string, KlipperTemperatureSensorHistory> CachedTemperatures { get; set; } = new();
#endif
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
