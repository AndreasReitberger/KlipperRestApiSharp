using Newtonsoft.Json;
#if ConcurrentDictionary
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperIgnoredJsonResultsChangedEventArgs : KlipperEventArgs
    {
        #region Properties
#if ConcurrentDictionary
        public ConcurrentDictionary<string, string> NewIgnoredJsonResults { get; set; } = new();
#else
        public Dictionary<string, string> NewIgnoredJsonResults { get; set; } = new();
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
