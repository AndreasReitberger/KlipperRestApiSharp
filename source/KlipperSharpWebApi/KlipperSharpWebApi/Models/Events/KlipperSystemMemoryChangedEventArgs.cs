using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public class KlipperSystemMemoryChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public Dictionary<string, long?> SystemMemory { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
