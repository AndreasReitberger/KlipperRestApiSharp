using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class KlipperToolHeadStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusToolhead ToolheadStates { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
