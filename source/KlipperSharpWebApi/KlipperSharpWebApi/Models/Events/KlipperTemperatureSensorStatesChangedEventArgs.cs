using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class KlipperTemperatureSensorStatesChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public Dictionary<string, KlipperStatusTemperatureSensor> TemperatureStates { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
