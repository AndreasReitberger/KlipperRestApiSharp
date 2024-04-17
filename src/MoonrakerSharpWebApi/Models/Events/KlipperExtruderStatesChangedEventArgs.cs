using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperExtruderStatesChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
#if ConcurrentDictionary
        public ConcurrentDictionary<int, KlipperStatusExtruder> ExtruderStates { get; set; } = new();
#else
        public Dictionary<int, KlipperStatusExtruder> ExtruderStates { get; set; } = new();
#endif
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
