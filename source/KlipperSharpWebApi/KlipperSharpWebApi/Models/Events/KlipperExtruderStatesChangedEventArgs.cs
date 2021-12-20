using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class KlipperExtruderStatesChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public Dictionary<int, KlipperStatusExtruder> ExtruderStates { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
