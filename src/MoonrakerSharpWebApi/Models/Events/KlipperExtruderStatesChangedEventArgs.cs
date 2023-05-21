using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperExtruderStatesChangedEventArgs : KlipperEventArgs
    {
        #region Properties
#if ConcurrentDictionary
        public ConcurrentDictionary<int, KlipperStatusExtruder> ExtruderStates { get; set; } = new();
#else
        public Dictionary<int, KlipperStatusExtruder> ExtruderStates { get; set; } = new();
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
