using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
#if ConcurrentDictionary
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperTemperatureSensorStatesChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
#if ConcurrentDictionary
        public ConcurrentDictionary<string, KlipperStatusTemperatureSensor> TemperatureStates { get; set; }
#else
        public Dictionary<string, KlipperStatusTemperatureSensor> TemperatureStates { get; set; }
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
